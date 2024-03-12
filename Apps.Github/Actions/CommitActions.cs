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
using Apps.Github.Models.Branch.Requests;
using Apps.GitHub.Models.Branch.Requests;
using Octokit;

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
    public ListRepositoryCommitsResponse ListRepositoryCommits(
        [ActionParameter] GetRepositoryRequest input,
        [ActionParameter] GetOptionalBranchRequest branchRequest)
    {
        var commits = Client.Repository.Commit.GetAll(long.Parse(input.RepositoryId), new CommitRequest() { Sha = branchRequest.Name}).Result;
        return new()
        {
            Commits = commits.Select(c => new SmallCommitDto(c))
        };
    }

    [Action("Get commit", Description = "Get commit by id")]
    public FullCommitDto GetCommit(
        [ActionParameter] GetRepositoryRequest repositoryRequest,
        [ActionParameter] GetCommitRequest input)
    {
        if (!long.TryParse(repositoryRequest.RepositoryId, out var intRepoId))
            throw new("Wrong repository ID");

        var commit = Client.Repository.Commit.Get(intRepoId, input.CommitId).Result;
        return new(commit);
    }

    [Action("Push file", Description = "Push file to repository")]
    public SmallCommitDto PushFile(
        [ActionParameter] GetRepositoryRequest repositoryRequest,
        [ActionParameter] GetOptionalBranchRequest branchRequest,
        [ActionParameter] PushFileRequest input)
    {
        var repContent = new RepositoryActions(InvocationContext, _fileManagementClient).ListAllRepositoryContent(
            new()
            {
                RepositoryId = repositoryRequest.RepositoryId,
            }, branchRequest);
        if (repContent.Items.Any(p => p.Path == input.DestinationFilePath)) // update in case of existing file
        {
            return UpdateFile(
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
        
        var file = _fileManagementClient.DownloadAsync(input.File).Result;
        var fileBytes = file.GetByteData().Result;

        var fileUpload =
            new Octokit.CreateFileRequest(input.CommitMessage, Convert.ToBase64String(fileBytes), branchRequest.Name, false);
        var pushFileResult = Client.Repository.Content
            .CreateFile(long.Parse(repositoryRequest.RepositoryId), input.DestinationFilePath, fileUpload).Result;
        return new(pushFileResult.Commit);
    }

    [Action("Update file", Description = "Update file in repository")]
    public SmallCommitDto UpdateFile(
        [ActionParameter] GetRepositoryRequest repositoryRequest,
        [ActionParameter] GetOptionalBranchRequest branchRequest,
        [ActionParameter] Models.Commit.Requests.UpdateFileRequest input)
    {
        var fileId = input.FileId ?? GetFileId(repositoryRequest.RepositoryId, input.DestinationFilePath, branchRequest);
        var file = _fileManagementClient.DownloadAsync(input.File).Result;
        var fileBytes = file.GetByteData().Result;
        var fileUpload = new Octokit.UpdateFileRequest(input.CommitMessage, Convert.ToBase64String(fileBytes), fileId, branchRequest.Name, 
            false);
        var pushFileResult = Client.Repository.Content
            .UpdateFile(long.Parse(repositoryRequest.RepositoryId), input.DestinationFilePath, fileUpload).Result;

        return new(pushFileResult.Commit);
    }

    [Action("Delete file", Description = "Delete file from repository")]
    public Task DeleteFile(
        [ActionParameter] GetRepositoryRequest repositoryRequest,
        [ActionParameter] GetOptionalBranchRequest branchRequest,
        [ActionParameter] Models.Commit.Requests.DeleteFileRequest input)
    {
        var fileId = GetFileId(repositoryRequest.RepositoryId, input.FilePath, branchRequest);

        var fileDelete = new Octokit.DeleteFileRequest(input.CommitMessage, fileId, branchRequest.Name);
        return Client.Repository.Content.DeleteFile(long.Parse(repositoryRequest.RepositoryId), input.FilePath, fileDelete);
    }

    private string GetFileId(string repoId, string path, GetOptionalBranchRequest branchRequest)
    {
        var repoContent = new RepositoryActions(InvocationContext, _fileManagementClient).ListAllRepositoryContent(new()
        {
            RepositoryId = repoId
        }, branchRequest);

        return repoContent.Items.First(x => x.Path == path).Sha;
    }
}