using Apps.Github.Actions.Base;
using Apps.GitHub.Models.Branch.Requests;
using Apps.Github.Models.Respository.Requests;
using Apps.GitHub.Models.Respository.Responses;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using System.Net.Mime;
using Apps.GitHub.Models.File.Requests;
using Apps.Github.Models.Commit.Requests;
using RestSharp;
using Blackbird.Applications.Sdk.Utils.Extensions.Files;
using Apps.GitHub.Dtos.Rest;
using Apps.GitHub.Dtos;
using Apps.GitHub.Extensions;

namespace Apps.GitHub.Actions;

//V2
[ActionList]
public class FileActions : GithubActions
{
    private readonly IFileManagementClient _fileManagementClient;

    public FileActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient)
        : base(invocationContext)
    {
        _fileManagementClient = fileManagementClient;
    }

   
    [Action("Download file", Description = "Download a file from a specified folder")]
    public async Task<FileReference> DownloadFile(
        [ActionParameter] GetRepositoryRequest repositoryRequest,
        [ActionParameter] GetOptionalBranchRequest branchRequest,
        [ActionParameter] DownloadFileRequest downloadFileRequest)
    {
        var repositoryInfo = await ClientSdk.Repository.Get(long.Parse(repositoryRequest.RepositoryId));
        var fileInfo = string.IsNullOrEmpty(branchRequest.Name)
            ? await ClientSdk.Repository.Content.GetAllContents(repositoryInfo.Owner.Login, repositoryInfo.Name, downloadFileRequest.FilePath)
            : await ClientSdk.Repository.Content.GetAllContentsByRef(repositoryInfo.Owner.Login, repositoryInfo.Name,
                downloadFileRequest.FilePath, branchRequest.Name);

        ValidateFileResponse(fileInfo, downloadFileRequest.FilePath);

        var filename = Path.GetFileName(downloadFileRequest.FilePath);
        if (!MimeTypes.TryGetMimeType(filename, out var mimeType))
            mimeType = MediaTypeNames.Application.Octet;

        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, fileInfo.First().DownloadUrl);
        FileReference fileReference = new FileReference(httpRequestMessage, filename, mimeType);
        return fileReference;
    }

    
    [Action("Search files in folder", Description = "Search files in folder. Note: the Github API limits to 1000 files per folder")]
    public async Task<SearchFileInFolderResponse> SearchFilesInFolder(
        [ActionParameter] GetRepositoryRequest repositoryRequest,
        [ActionParameter] GetOptionalBranchRequest branchRequest,
        [ActionParameter] SearchFilesRequest folderContentRequest)
    {
        var repositoryId = long.Parse(repositoryRequest.RepositoryId);

        var reference = branchRequest.Name;        
        if (reference == null)
        {
            var branchResult = await ClientSdk.Repository.Get(repositoryId);
            reference = branchResult.DefaultBranch;
        }

        if (folderContentRequest.Path != null)
        {
            reference = reference + ":" + folderContentRequest.Path;
        }

        var res = (folderContentRequest.IncludeSubfolders.HasValue && folderContentRequest.IncludeSubfolders.Value) ? 
            await ClientSdk.Git.Tree.GetRecursive(long.Parse(repositoryRequest.RepositoryId), reference) : 
            await ClientSdk.Git.Tree.Get(long.Parse(repositoryRequest.RepositoryId), reference);

        return new SearchFileInFolderResponse(res, folderContentRequest.Path, folderContentRequest.Filter) { Truncated = res.Truncated };

        //var folderContent = (string.IsNullOrEmpty(branchRequest.Name)
        //    ? await ClientSdk.Repository.Content.GetAllContents(long.Parse(repositoryRequest.RepositoryId),
        //        folderContentRequest.Path ?? "/")
        //    : await ClientSdk.Repository.Content.GetAllContentsByRef(long.Parse(repositoryRequest.RepositoryId),
        //        folderContentRequest.Path ?? "/", branchRequest.Name)).ToList();

        //var result = new SearchFileInFolderResponse(folderContent, folderContentRequest.Filter);

        //if (folderContentRequest.IncludeSubfolders.HasValue && folderContentRequest.IncludeSubfolders.Value)
        //{
        //    foreach (var folder in folderContent.Where(x => x.Type.Value == Octokit.ContentType.Dir).ToList())
        //    {
        //        var innerContent = await SearchFilesInFolder(repositoryRequest, branchRequest,
        //            new(folder.Path, true));
        //        result.Files.AddRange(innerContent.Files);
        //    }
        //}
        //return result;
    }

    [Action("File exists", Description = "Check if file exists in repository")]
    public async Task<bool> FileExists(
        [ActionParameter] GetRepositoryRequest repositoryRequest,
        [ActionParameter] GetOptionalBranchRequest branchRequest,
        [ActionParameter] DownloadFileRequest getFileRequest)
    {
        try
        {
            var repoInfo = await ClientSdk.Repository.Get(long.Parse(repositoryRequest.RepositoryId));
            var fileData = string.IsNullOrEmpty(branchRequest.Name)
                ? await ClientSdk.Repository.Content.GetRawContent(repoInfo.Owner.Login, repoInfo.Name, getFileRequest.FilePath)
                : await ClientSdk.Repository.Content.GetRawContentByRef(repoInfo.Owner.Login, repoInfo.Name,
                    getFileRequest.FilePath, branchRequest.Name);

            if (fileData == null)
            {
                return false;
            }

            return true;
        }
        catch (Exception e)
        {
            if (e.Message.Contains("Not Found"))
            {
                return false;
            }

            throw;
        }
    }

    [Action("Create or update file", Description = "Create or update file")]
    public async Task CreateOrUpdateFile(
        [ActionParameter] GetRepositoryRequest repositoryRequest,
        [ActionParameter] GetOptionalBranchRequest branchRequest,
        [ActionParameter] CreateOrUpdateFileRequest createOrUpdateRequest)
    {
        var file = await _fileManagementClient.DownloadAsync(createOrUpdateRequest.File);
        var fileBytes = await file.GetByteData();
        var repositoryInfo = await ClientSdk.Repository.Get(long.Parse(repositoryRequest.RepositoryId));
        var filePath = $"{createOrUpdateRequest.Folder.TrimEnd('/')}/{createOrUpdateRequest.File.Name.TrimStart('/')}";

        var fileContentDto = new FileContentDto();
        try
        {
            var getFileRequest = new RestRequest($"/{repositoryInfo.Owner.Login}/{repositoryInfo.Name}/contents/{filePath}", Method.Get);
            getFileRequest.AddGithubBranch(branchRequest);
            fileContentDto = await ClientRest.ExecuteWithErrorHandling<FileContentDto>(getFileRequest);
        }
        catch(GithubErrorException ex)
        {
            var createFileRequest = new RestRequest($"/{repositoryInfo.Owner.Login}/{repositoryInfo.Name}/contents/{filePath}", Method.Put);
            createFileRequest.AddBody(new
            {
                message = createOrUpdateRequest.CommitMessage,
                content = Convert.ToBase64String(fileBytes)
            });
            createFileRequest.AddGithubBranch(branchRequest);
            await ClientRest.ExecuteWithErrorHandling(createFileRequest);
            return;
        }
        var updateFileRequest = new RestRequest($"/{repositoryInfo.Owner.Login}/{repositoryInfo.Name}/contents/{filePath}", Method.Put);
        updateFileRequest.AddGithubBranch(branchRequest);
        updateFileRequest.AddBody(new
        {
            message = createOrUpdateRequest.CommitMessage,
            content = Convert.ToBase64String(fileBytes),
            sha = fileContentDto.Sha
        });
        await ClientRest.ExecuteWithErrorHandling(updateFileRequest);
    }

    [Action("Delete file", Description = "Delete file from repository")]
    public async Task DeleteFile(
        [ActionParameter] GetRepositoryRequest repositoryRequest,
        [ActionParameter] GetOptionalBranchRequest branchRequest,
        [ActionParameter] DeleteFileRequest deleteFileRequest)
    {
        var repositoryInfo = await ClientSdk.Repository.Get(long.Parse(repositoryRequest.RepositoryId));
        var fileInfo = string.IsNullOrEmpty(branchRequest.Name)
            ? await ClientSdk.Repository.Content.GetAllContents(repositoryInfo.Owner.Login, repositoryInfo.Name, deleteFileRequest.FilePath)
            : await ClientSdk.Repository.Content.GetAllContentsByRef(repositoryInfo.Owner.Login, repositoryInfo.Name,
                deleteFileRequest.FilePath, branchRequest.Name);

        ValidateFileResponse(fileInfo, deleteFileRequest.FilePath);

        var fileDelete = new Octokit.DeleteFileRequest(deleteFileRequest.CommitMessage, fileInfo.First().Sha, branchRequest.Name);
        await ClientSdk.Repository.Content.DeleteFile(long.Parse(repositoryRequest.RepositoryId), deleteFileRequest.FilePath,
            fileDelete);
    }

    private void ValidateFileResponse(IReadOnlyList<Octokit.RepositoryContent> repositoryContent, string filePath)
    {
        if (repositoryContent == null || repositoryContent?.Count == 0)
            throw new ArgumentException($"File does not exist ({filePath})");
        if (repositoryContent.First().Size == 0)
            throw new ArgumentException($"File is empty ({filePath})");
    }
}
