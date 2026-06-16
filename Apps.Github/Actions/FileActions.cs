using Apps.Github.Models.Commit.Requests;
using Apps.Github.Models.Respository.Requests;
using Apps.GitHub.Api;
using Apps.GitHub.Dtos;
using Apps.GitHub.Dtos.Rest;
using Apps.GitHub.Extensions;
using Apps.GitHub.Models.Branch.Requests;
using Apps.GitHub.Models.File.Requests;
using Apps.GitHub.Models.File.Responses;
using Apps.GitHub.Models.Respository.Responses;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Extensions.Files;
using Blackbird.Applications.SDK.Extensions.FileManagement;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using Blackbird.Filters.Coders;
using Blackbird.Filters.Constants;
using Blackbird.Filters.Enums;
using Blackbird.Filters.Extensions;
using Blackbird.Filters.Transformations;
using Blackbird.Filters.Xliff.Xliff1;
using Blackbird.Filters.Xliff.Xliff2;
using RestSharp;
using System.IO;
using System.Net.Mime;
using System.Text;

namespace Apps.GitHub.Actions;

[ActionList("Files")]
public class FileActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient)
    : GithubInvocable(invocationContext)
{
    [Action("Download file", Description = "Download a file from a specified folder")]
    public async Task<FileResponse> DownloadFile(
        [ActionParameter] GetRepositoryRequest repositoryRequest,
        [ActionParameter] GetOptionalBranchRequest branchRequest,
        [ActionParameter] DownloadFileRequest downloadFileRequest)
    {
        var repositoryInfo = await ExecuteWithErrorHandlingAsync(async () =>
            await ClientSdk.Repository.Get(long.Parse(repositoryRequest
                .RepositoryId)));

        var file = await GetFileInfo(repositoryInfo, branchRequest, downloadFileRequest.FilePath);

        if (file.Content != null)
        {
            return await DownloadBlackbirdInteroperableFile(file);
        }

        return DownloadFileOnCore(file);
    }

    private async Task<Octokit.RepositoryContent> GetFileInfo(Octokit.Repository repositoryInfo, GetOptionalBranchRequest branchRequest, string filePath)
    {
        var fileInfo = string.IsNullOrEmpty(branchRequest?.Name)
           ? await ExecuteWithErrorHandlingAsync(async () =>
               await ClientSdk.Repository.Content.GetAllContents(repositoryInfo.Owner.Login, repositoryInfo.Name, filePath))
           : await ExecuteWithErrorHandlingAsync(async () => await ClientSdk.Repository.Content.GetAllContentsByRef(
               repositoryInfo.Owner.Login, repositoryInfo.Name,
               filePath, branchRequest?.Name));

        ValidateFileResponse(fileInfo, filePath);
        return fileInfo.First();
    }

    private FileResponse DownloadFileOnCore(Octokit.RepositoryContent file)
    {
        var filename = Path.GetFileName(file.Path);
        if (!MimeTypes.TryGetMimeType(filename, out var mimeType))
            mimeType = MediaTypeNames.Application.Octet;

        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, file.DownloadUrl);
        var fileReference = new FileReference(httpRequestMessage, filename, mimeType);
        return new FileResponse { Content = fileReference };
    }

    private async Task<FileResponse> DownloadBlackbirdInteroperableFile(Octokit.RepositoryContent file, string? language = null)
    {
        var content = file.Content;
        var filename = Path.GetFileName(file.Path);
        if (!MimeTypes.TryGetMimeType(filename, out var mimeType))
            mimeType = MediaTypeNames.Application.Octet;

        var transformationResult = Transformation.Load(content.ToStream(), filename, mimeType);
        if (!transformationResult.Success)
        {
            var directFileReference = await fileManagementClient.UploadAsync(new MemoryStream(Encoding.UTF8.GetBytes(content)), mimeType, file.Name);
            return new FileResponse { Content = directFileReference };
        }

        var transformation = transformationResult.Value!;

        transformation.SourceLanguage = language ?? file.Name.Split('.')[0];
        transformation.SourceSystemReference.ContentId = (new Uri(file.HtmlUrl)).AbsolutePath;
        transformation.SourceSystemReference.AdminUrl = file.HtmlUrl;
        transformation.SourceSystemReference.ContentName = file.Name;
        transformation.SourceSystemReference.SystemName = "Github";
        transformation.SourceSystemReference.SystemRef = "https://github.com/";

        var fileReference = await fileManagementClient.UploadAsync(transformation.Source().ToStream(), mimeType, file.Name);

        return new FileResponse { Content = fileReference };
    }


    [Action("Search files in folder",
        Description = "Search files in folder. Note: the Github API limits to 1000 files per folder")]
    public async Task<SearchFileInFolderResponse> SearchFilesInFolder(
        [ActionParameter] GetRepositoryRequest repositoryRequest,
        [ActionParameter] GetOptionalBranchRequest branchRequest,
        [ActionParameter] SearchFilesRequest folderContentRequest)
    {
        var repositoryId = long.Parse(repositoryRequest.RepositoryId);

        var reference = branchRequest?.Name;
        if (reference == null)
        {
            var branchResult =
                await ExecuteWithErrorHandlingAsync(async () => await ClientSdk.Repository.Get(repositoryId));
            reference = branchResult.DefaultBranch;
        }

        if (folderContentRequest.Path != null)
        {
            reference = reference + ":" + folderContentRequest.Path.TrimEnd('/');
        }

        var res = (folderContentRequest.IncludeSubfolders.HasValue && folderContentRequest.IncludeSubfolders.Value)
            ? await ExecuteWithErrorHandlingAsync(async () =>
                await ClientSdk.Git.Tree.GetRecursive(long.Parse(repositoryRequest.RepositoryId), reference))
            : await ExecuteWithErrorHandlingAsync(async () =>
                await ClientSdk.Git.Tree.Get(long.Parse(repositoryRequest.RepositoryId), reference));

        return new SearchFileInFolderResponse(res, folderContentRequest.Path?.TrimEnd('/'), folderContentRequest.Filter)
            { Truncated = res.Truncated };
    }

    [Action("File exists", Description = "Check if file exists in repository")]
    public async Task<bool> FileExists(
        [ActionParameter] GetRepositoryRequest repositoryRequest,
        [ActionParameter] GetOptionalBranchRequest branchRequest,
        [ActionParameter] DownloadFileRequest getFileRequest)
    {
        var repositoryInfo = await ExecuteWithErrorHandlingAsync(async () =>
            await ClientSdk.Repository.Get(long.Parse(repositoryRequest.RepositoryId)));
        var branchName = string.IsNullOrEmpty(branchRequest?.Name) ? repositoryInfo.DefaultBranch : branchRequest.Name;
        var treeResponse =
            await ExecuteWithErrorHandlingAsync(async () =>
                await ClientSdk.Git.Tree.Get(long.Parse(repositoryRequest.RepositoryId),
                    $"{branchName}:{Path.GetDirectoryName(getFileRequest.FilePath)}"));
        return treeResponse.Tree.Any(x => x.Path == Path.GetFileName(getFileRequest.FilePath));
    }

    [Action("Upload file", Description = "Uploads a file, creates a new file if it doesn't exist otherwise updates the file")]
    public async Task<FileResponse> CreateOrUpdateFile(
        [ActionParameter] GetRepositoryRequest repositoryRequest,
        [ActionParameter] GetOptionalBranchRequest branchRequest,
        [ActionParameter] CreateOrUpdateFileRequest createOrUpdateRequest)
    {
        var file = await fileManagementClient.DownloadAsync(createOrUpdateRequest.File);
        var repositoryInfo = await ExecuteWithErrorHandlingAsync(async () =>
            await ClientSdk.Repository.Get(long.Parse(repositoryRequest.RepositoryId)));
        var oldFileName = createOrUpdateRequest!.File.Name;

        bool isJson = oldFileName.EndsWith(".json", StringComparison.OrdinalIgnoreCase);
        string? content = null;
        var transformationResult = Transformation.Load(file, createOrUpdateRequest.File.Name, createOrUpdateRequest.File.ContentType);
        if (transformationResult.Success && !isJson)
        {
            content = transformationResult.Value.Target().ToStream(MetadataHandling.Exclude).ReadString();
        }
        else
        {            
            content = Encoding.UTF8.GetString(await file.GetByteData());
        }

        if (isJson)
        {
            content = transformationResult.Value.Source().ToStream(MetadataHandling.Exclude).ReadString();
            //string pattern = @",\s*""__blackbird_meta""[\s\S]+$";

            //content = System.Text.RegularExpressions.Regex.Replace(content, pattern, "\n}"); 
        }

        var fileName = GetNewFileName(oldFileName, createOrUpdateRequest.NewFileName);
        var filePath = $"{createOrUpdateRequest?.FolderPath?.TrimEnd('/')}/{fileName.TrimStart('/')}".TrimStart('/');
        var url = $"/{repositoryInfo.Owner.Login}/{repositoryInfo.Name}/contents/{filePath}";

        try
        {
            var getFileRequest = new RestRequest(url);
            getFileRequest.AddGithubBranch(branchRequest);

            var fileContentDto = new FileContentDto();
            try
            {
                fileContentDto = await ClientRest.ExecuteWithErrorHandling<FileContentDto>(getFileRequest);
            }
            catch (GithubErrorException ex)
            {
                if (ex.ErrorCode != 404)
                {
                    throw;
                }
            }

            var createFileDictionary = new Dictionary<string, object>
            {
                { "message", createOrUpdateRequest!.CommitMessage },
                { "content", Convert.ToBase64String(Encoding.UTF8.GetBytes(content))},
                { "sha", fileContentDto?.Sha ?? string.Empty }
            };

            if (!string.IsNullOrEmpty(branchRequest.Name))
            {
                createFileDictionary.Add("branch", branchRequest.Name);
            }

            var createFileRequest = new RestRequest(url, Method.Put)
                .AddBody(createFileDictionary);

            await ClientRest.ExecuteWithErrorHandling(createFileRequest);
        }
        catch (GithubErrorException ex)
        {
            throw new PluginApplicationException($"Error creating or updating file: {ex.Message}");
        }
        catch (HttpRequestException ex)
        {
            throw new PluginApplicationException($"Error creating or updating file: {ex.Message}");
        }

        var output = new FileResponse { Content = createOrUpdateRequest.File };

        if (transformationResult.Success && !isJson)
        {
            var uploadedFileInfo = await GetFileInfo(repositoryInfo, branchRequest, filePath);
            var transformation = transformationResult.Value;

            transformation.TargetSystemReference.ContentId = (new Uri(uploadedFileInfo.HtmlUrl)).AbsolutePath;
            transformation.TargetSystemReference.ContentName = uploadedFileInfo.Name;
            transformation.TargetSystemReference.AdminUrl = uploadedFileInfo.HtmlUrl;
            transformation.TargetSystemReference.SystemName = "Github";
            transformation.TargetSystemReference.SystemRef = "https://github.com/";
            transformation.TargetLanguage = fileName.Split('.')[0];

            output.Content = await fileManagementClient.UploadAsync(
                transformation.ToStream(),
                MediaTypes.Xliff2,
                transformation.BilingualFileName);
        }

        return output;
    }

    [Action("Delete file", Description = "Delete file from repository")]
    public async Task DeleteFile(
        [ActionParameter] GetRepositoryRequest repositoryRequest,
        [ActionParameter] GetOptionalBranchRequest branchRequest,
        [ActionParameter] DeleteFileRequest deleteFileRequest)
    {
        var repositoryInfo = await ExecuteWithErrorHandlingAsync(async () =>
            await ClientSdk.Repository.Get(long.Parse(repositoryRequest.RepositoryId)));
        var branchName = string.IsNullOrEmpty(branchRequest?.Name) ? repositoryInfo.DefaultBranch : branchRequest.Name;
        var treeResponse =
            await ExecuteWithErrorHandlingAsync(async () =>
                await ClientSdk.Git.Tree.Get(long.Parse(repositoryRequest.RepositoryId),
                    $"{branchName}:{Path.GetDirectoryName(deleteFileRequest.FilePath)}"));
        var fileInfo = treeResponse.Tree.FirstOrDefault(x => x.Path == Path.GetFileName(deleteFileRequest.FilePath));
        if (fileInfo == null)
            throw new PluginApplicationException($"File does not exist ({deleteFileRequest.FilePath})");

        var fileDelete = new Octokit.DeleteFileRequest(deleteFileRequest.CommitMessage, fileInfo.Sha, branchName);
        await ExecuteWithErrorHandlingAsync(async () => await ClientSdk.Repository.Content.DeleteFile(
            long.Parse(repositoryRequest.RepositoryId), deleteFileRequest.FilePath,
            fileDelete));
    }

    [Action("Download repository as zip", Description = "Download repository as zip")]
    public async Task<FileResponse> DownloadRepositoryZip(
        [ActionParameter] GetRepositoryRequest repositoryRequest,
        [ActionParameter] GetOptionalBranchRequest branchRequest)
    {
        var repositoryInfo = await ExecuteWithErrorHandlingAsync(async () =>
            await ClientSdk.Repository.Get(long.Parse(repositoryRequest.RepositoryId)));
        var branchRef = string.IsNullOrEmpty(branchRequest?.Name) ? string.Empty : branchRequest.Name;
        var getZipRequest = new RestRequest($"/{repositoryInfo.Owner.Login}/{repositoryInfo.Name}/zipball/{branchRef}",
            Method.Get);

        var customRestClient = GetUnfollowRedirectsClient();
        var getZipResponse = await customRestClient.ExecuteAsync(getZipRequest);

        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get,
            getZipResponse?.Headers?.FirstOrDefault(x => x.Name == "Location")?.Value?.ToString());
        FileReference fileReference = new FileReference(httpRequestMessage, $"{repositoryInfo.Name}.zip",
            MediaTypeNames.Application.Zip);
        return new FileResponse { Content = fileReference };
    }

    private void ValidateFileResponse(IReadOnlyList<Octokit.RepositoryContent> repositoryContent, string filePath)
    {
        if (repositoryContent == null || repositoryContent?.Count == 0)
            throw new PluginApplicationException($"File does not exist ({filePath})");
        if (repositoryContent?.First().Size == 0)
            throw new PluginApplicationException($"File is empty ({filePath})");
    }

    private GithubRestClient GetUnfollowRedirectsClient()
    {
        return new GithubRestClient(InvocationContext.AuthenticationCredentialsProviders, new()
        {
            BaseUrl = new Uri("https://api.github.com/repos"),
            FollowRedirects = false,
        });
    }

    private string GetNewFileName(string oldFileName, string? newFileName)
    {
        if (newFileName is null) return oldFileName;

        if (newFileName.Contains('.'))
        {
            return newFileName;
        }

        return newFileName + '.' + oldFileName.Split('.').Last();
    }
}