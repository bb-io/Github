﻿using Apps.GitHub.Models.Branch.Requests;
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
using Apps.GitHub.Api;
using Blackbird.Applications.Sdk.Common.Exceptions;

namespace Apps.GitHub.Actions;

[ActionList]
public class FileActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient)
    : GithubInvocable(invocationContext)
{
    [Action("Download file", Description = "Download a file from a specified folder")]
    public async Task<FileReference> DownloadFile(
        [ActionParameter] GetRepositoryRequest repositoryRequest,
        [ActionParameter] GetOptionalBranchRequest branchRequest,
        [ActionParameter] DownloadFileRequest downloadFileRequest)
    {
        var repositoryInfo = await ExecuteWithErrorHandlingAsync(async () =>
            await ClientSdk.Repository.Get(long.Parse(repositoryRequest
                .RepositoryId))); //not found file needs to be addressed
        var fileInfo = string.IsNullOrEmpty(branchRequest?.Name)
            ? await ExecuteWithErrorHandlingAsync(async () =>
                await ClientSdk.Repository.Content.GetAllContents(repositoryInfo.Owner.Login, repositoryInfo.Name,
                    downloadFileRequest.FilePath))
            : await ExecuteWithErrorHandlingAsync(async () => await ClientSdk.Repository.Content.GetAllContentsByRef(
                repositoryInfo.Owner.Login, repositoryInfo.Name,
                downloadFileRequest.FilePath, branchRequest?.Name));

        ValidateFileResponse(fileInfo, downloadFileRequest.FilePath);

        var filename = Path.GetFileName(downloadFileRequest.FilePath);
        if (!MimeTypes.TryGetMimeType(filename, out var mimeType))
            mimeType = MediaTypeNames.Application.Octet;

        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, fileInfo.First().DownloadUrl);
        var fileReference = new FileReference(httpRequestMessage, filename, mimeType);
        return fileReference;
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

    [Action("Create or update file", Description = "Create or update file")]
    public async Task CreateOrUpdateFile(
        [ActionParameter] GetRepositoryRequest repositoryRequest,
        [ActionParameter] GetOptionalBranchRequest branchRequest,
        [ActionParameter] CreateOrUpdateFileRequest createOrUpdateRequest)
    {
        var file = await fileManagementClient.DownloadAsync(createOrUpdateRequest.File);
        var fileBytes = await file.GetByteData();
        var repositoryInfo = await ExecuteWithErrorHandlingAsync(async () =>
            await ClientSdk.Repository.Get(long.Parse(repositoryRequest.RepositoryId)));
        var filePath = string.IsNullOrWhiteSpace(Path.GetExtension(createOrUpdateRequest?.FilePath))
            ? $"{createOrUpdateRequest?.FilePath?.TrimEnd('/')}/{createOrUpdateRequest?.File?.Name?.TrimStart('/')}"
            : createOrUpdateRequest?.FilePath;
        filePath = filePath?.TrimStart('/');
        var url = $"/{repositoryInfo.Owner.Login}/{repositoryInfo.Name}/contents/{filePath}";

        var fileContentDto = new FileContentDto();
        try
        {
            var getFileRequest = new RestRequest(url);
            getFileRequest.AddGithubBranch(branchRequest);

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
                { "content", Convert.ToBase64String(fileBytes)},
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
    }

    [Action("Delete file", Description = "Delete file from repository")]
    public async Task DeleteFile(
        [ActionParameter] GetRepositoryRequest repositoryRequest,
        [ActionParameter] GetOptionalBranchRequest branchRequest,
        [ActionParameter] Models.File.Requests.DeleteFileRequest deleteFileRequest)
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
    public async Task<FileReference> DownloadRepositoryZip(
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
        return fileReference;
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
}