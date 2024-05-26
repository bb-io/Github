using Apps.Github.Actions.Base;
using Apps.Github.Dtos;
using Apps.Github.Models.Commit.Requests;
using Apps.Github.Models.Commit.Responses;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using Blackbird.Applications.Sdk.Utils.Extensions.Files;
using Apps.Github.Models.Respository.Requests;
using Apps.GitHub.Models.Branch.Requests;
using Octokit;
using Apps.GitHub.Models.Commit.Requests;
using Apps.Github.Webhooks.Payloads;
using Apps.Github.Webhooks;
using Apps.GitHub.Models.Commit.Responses;
using Blackbird.Applications.Sdk.Common.Files;
using System.Net.Mime;

namespace Apps.Github.Actions;

[ActionList]
public class CommitActions : GithubActions
{
    private readonly IFileManagementClient _fileManagementClient;

    public CommitActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient)
        : base(invocationContext)
    {
        _fileManagementClient = fileManagementClient;
    }

    [Action("List commits", Description = "List respository commits")]
    public async Task<ListRepositoryCommitsResponse> ListRepositoryCommits(
        [ActionParameter] GetRepositoryRequest input,
        [ActionParameter] GetOptionalBranchRequest branchRequest)
    {
        var commits = await Client.Repository.Commit.GetAll(long.Parse(input.RepositoryId),
            new CommitRequest() { Sha = branchRequest.Name });
        return new()
        {
            Commits = commits.Select(c => new SmallCommitDto(c))
        };
    }

    [Action("List added or modified files in X hours", Description = "List added or modified files in X hours")]
    public async Task<ListAddedOrModifiedInHoursResponse> ListAddedOrModifiedInHours(
        [ActionParameter] GetRepositoryRequest input,
        [ActionParameter] GetOptionalBranchRequest branchRequest,
        [ActionParameter] AddedOrModifiedHoursRequest hoursRequest,
        [ActionParameter] FolderInput folderInput)
    {
        if (hoursRequest.Hours <= 0)
            throw new ArgumentException("Specify more than 0 hours!");
        var commits = await Client.Repository.Commit.GetAll(long.Parse(input.RepositoryId), new CommitRequest()
        {
            Sha = branchRequest.Name,
            Since = DateTime.Now.AddHours(-hoursRequest.Hours),
        });
        var commitsList = commits.ToList();
        var files = new List<GitHubCommitFile>();
        commitsList.ForEach(c =>
        {
            var commit = Client.Repository.Commit.Get(long.Parse(input.RepositoryId), c.Sha).Result;
            if (hoursRequest.Authors != null && !hoursRequest.Authors.Contains(commit.Author.Login) &&
            (!hoursRequest.ExcludeAuthors.HasValue || !hoursRequest.ExcludeAuthors.Value))
                return;
            else if (hoursRequest.Authors != null && hoursRequest.Authors.Contains(commit.Author.Login) &&
            (hoursRequest.ExcludeAuthors.HasValue && hoursRequest.ExcludeAuthors.Value))
                return;
            else if (hoursRequest.ExcludeMerge.HasValue && hoursRequest.ExcludeMerge.Value && c.Parents.Count > 1)
                return;
            files.AddRange(commit.Files.Where(x => new[] { "added", "modified" }.Contains(x.Status)).Where(f => folderInput.FolderPath is null || PushWebhooks.IsFilePathMatchingPattern(folderInput.FolderPath, f.Filename)));
        });

        return new(files.DistinctBy(x => x.Filename).Select(x => new CommitFileDto(x)).ToList());
    }

    [Action("Download added or modified files in X hours as zip", Description = "Download added or modified files in X hours as zip")]
    public async Task<FileReference> DownloadAddedOrModifiedInHours(  // previous return type ListAddedOrModifiedInHoursResponse
        [ActionParameter] GetRepositoryRequest repositoryRequest,
        [ActionParameter] GetOptionalBranchRequest branchRequest,
        [ActionParameter] AddedOrModifiedHoursRequest hoursRequest,
        [ActionParameter] FolderInput folderInput)
    {
        if (hoursRequest.Hours <= 0)
            throw new ArgumentException("Specify more than 0 hours!");
        var commits = await Client.Repository.Commit.GetAll(long.Parse(repositoryRequest.RepositoryId), new CommitRequest()
        {
            Sha = branchRequest.Name,
            Since = DateTime.Now.AddHours(-hoursRequest.Hours),
        });
        var commitsList = commits.ToList();
        var files = new List<GitHubCommitFile>();
        commitsList.ForEach(c =>
        {
            var commit = Client.Repository.Commit.Get(long.Parse(repositoryRequest.RepositoryId), c.Sha).Result;
            if (hoursRequest.Authors != null && !hoursRequest.Authors.Contains(commit.Author.Login) &&
            (!hoursRequest.ExcludeAuthors.HasValue || !hoursRequest.ExcludeAuthors.Value))
                return;
            else if (hoursRequest.Authors != null && hoursRequest.Authors.Contains(commit.Author.Login) &&
            (hoursRequest.ExcludeAuthors.HasValue && hoursRequest.ExcludeAuthors.Value))
                return;
            else if (hoursRequest.ExcludeMerge.HasValue && hoursRequest.ExcludeMerge.Value && c.Parents.Count > 1)
                return;
            files.AddRange(commit.Files.Where(x => new[] { "added", "modified" }.Contains(x.Status)).Where(f => folderInput.FolderPath is null || PushWebhooks.IsFilePathMatchingPattern(folderInput.FolderPath, f.Filename)));
        });
        var addedOrModifiedFilenames = files.DistinctBy(x => x.Filename).Select(x => new CommitFileDto(x)).Select(x => x.Filename).ToList();

        var content = string.IsNullOrEmpty(branchRequest.Name)
            ? await Client.Repository.Content.GetArchive(long.Parse(repositoryRequest.RepositoryId),
                ArchiveFormat.Zipball)
            : await Client.Repository.Content.GetArchive(long.Parse(repositoryRequest.RepositoryId),
                ArchiveFormat.Zipball, branchRequest.Name);

        using var sourceStream = new MemoryStream(content);
        var archive = SharpCompress.Archives.Zip.ZipArchive.Open(sourceStream);
        foreach (var entry in archive.Entries)
        {
            if (!entry.IsDirectory && !addedOrModifiedFilenames.Contains(entry.Key.Substring(entry.Key.IndexOf("/") + 1)))
                archive.RemoveEntry(entry);
        }
        using var resultStream = new MemoryStream();
        archive.SaveTo(resultStream);
        resultStream.Seek(0, SeekOrigin.Begin);
        var uploadedFile = await _fileManagementClient.UploadAsync(resultStream, MediaTypeNames.Application.Zip, $"Repository_{repositoryRequest.RepositoryId}.zip");
        return uploadedFile;
    }

    [Action("Get commit", Description = "Get commit by id")]
    public async Task<FullCommitDto> GetCommit(
        [ActionParameter] GetRepositoryRequest repositoryRequest,
        [ActionParameter] GetCommitRequest input)
    {
        if (!long.TryParse(repositoryRequest.RepositoryId, out var intRepoId))
            throw new("Wrong repository ID");

        var commit = await Client.Repository.Commit.Get(intRepoId, input.CommitId);
        return new(commit);
    }

    [Action("Create or update file", Description = "Create or update file")]
    public async Task<SmallCommitDto> PushFile(
        [ActionParameter] GetRepositoryRequest repositoryRequest,
        [ActionParameter] GetOptionalBranchRequest branchRequest,
        [ActionParameter] PushFileRequest input)
    {
        var repContent = await new RepositoryActions(InvocationContext, _fileManagementClient).ListAllRepositoryContent(
            new()
            {
                RepositoryId = repositoryRequest.RepositoryId,
            }, branchRequest);
        if (repContent.Items.Any(p => p.Path == input.DestinationFilePath)) // update in case of existing file
        {
            return await UpdateFile(
                repositoryRequest,
                branchRequest,
                new()
                {
                    FileId = repContent.Items.First(p => p.Path == input.DestinationFilePath).Sha,
                    DestinationFilePath = input.DestinationFilePath,
                    File = input.File,
                    CommitMessage = input.CommitMessage
                });
        }

        var file = await _fileManagementClient.DownloadAsync(input.File);
        var fileBytes = await file.GetByteData();

        var fileUpload =
            new Octokit.CreateFileRequest(input.CommitMessage, Convert.ToBase64String(fileBytes), branchRequest.Name,
                false);
        var pushFileResult = await Client.Repository.Content
            .CreateFile(long.Parse(repositoryRequest.RepositoryId), input.DestinationFilePath, fileUpload);
        return new(pushFileResult.Commit);
    }

    [Action("Update file", Description = "Update file in repository")]
    public async Task<SmallCommitDto> UpdateFile(
        [ActionParameter] GetRepositoryRequest repositoryRequest,
        [ActionParameter] GetOptionalBranchRequest branchRequest,
        [ActionParameter] Models.Commit.Requests.UpdateFileRequest input)
    {
        var fileId = input.FileId ??
                     await GetFileId(repositoryRequest.RepositoryId, input.DestinationFilePath, branchRequest);
        var file = await _fileManagementClient.DownloadAsync(input.File);
        var fileBytes = await file.GetByteData();
        var fileUpload = new Octokit.UpdateFileRequest(input.CommitMessage, Convert.ToBase64String(fileBytes), fileId,
            branchRequest.Name,
            false);
        var pushFileResult = await Client.Repository.Content
            .UpdateFile(long.Parse(repositoryRequest.RepositoryId), input.DestinationFilePath, fileUpload);

        return new(pushFileResult.Commit);
    }

    [Action("Delete file", Description = "Delete file from repository")]
    public async Task DeleteFile(
        [ActionParameter] GetRepositoryRequest repositoryRequest,
        [ActionParameter] GetOptionalBranchRequest branchRequest,
        [ActionParameter] Models.Commit.Requests.DeleteFileRequest input)
    {
        var fileId = await GetFileId(repositoryRequest.RepositoryId, input.FilePath, branchRequest);

        var fileDelete = new Octokit.DeleteFileRequest(input.CommitMessage, fileId, branchRequest.Name);
        await Client.Repository.Content.DeleteFile(long.Parse(repositoryRequest.RepositoryId), input.FilePath,
            fileDelete);
    }

    private async Task<string> GetFileId(string repoId, string path, GetOptionalBranchRequest branchRequest)
    {
        var repoContent =
            await new RepositoryActions(InvocationContext, _fileManagementClient).ListAllRepositoryContent(new()
            {
                RepositoryId = repoId
            }, branchRequest);

        return repoContent.Items.First(x => x.Path == path).Sha;
    }
}