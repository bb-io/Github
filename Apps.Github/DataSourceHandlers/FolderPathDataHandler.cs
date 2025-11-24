using Octokit;
using Apps.GitHub.Models.Branch.Requests;
using Apps.Github.Models.Respository.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using Blackbird.Applications.SDK.Extensions.FileManagement.Models.FileDataSourceItems;
using File = Blackbird.Applications.SDK.Extensions.FileManagement.Models.FileDataSourceItems.File;

namespace Apps.GitHub.DataSourceHandlers;

public class FolderPathDataHandler(
    InvocationContext invocationContext,
    [ActionParameter] GetRepositoryRequest repositoryRequest,
    [ActionParameter] GetOptionalBranchRequest branchRequest)
    : GithubInvocable(invocationContext), IAsyncFileDataSourceItemHandler
{
    public async Task<IEnumerable<FolderPathItem>> GetFolderPathAsync(FolderPathDataSourceContext context, CancellationToken token)
    {
        var itemId = context.FileDataItemId;

        var breadcrumb = new List<FolderPathItem>
        {
            new() {
                Id = "",
                DisplayName = "Root"
            }
        };

        if (!string.IsNullOrEmpty(itemId))
        {
            var folderPath = itemId;
            if (!itemId.EndsWith('/') && Path.HasExtension(itemId))
                folderPath = Path.GetDirectoryName(itemId)?.Replace("\\", "/");

            if (!string.IsNullOrEmpty(folderPath))
            {
                var parts = folderPath.Split('/');
                var currentPathAccumulator = "";

                foreach (var part in parts)
                {
                    if (string.IsNullOrEmpty(currentPathAccumulator))
                        currentPathAccumulator = part;
                    else
                        currentPathAccumulator += $"/{part}";

                    breadcrumb.Add(new FolderPathItem
                    {
                        Id = currentPathAccumulator,
                        DisplayName = part
                    });
                }
            }
        }

        return breadcrumb;
    }

    public async Task<IEnumerable<FileDataItem>> GetFolderContentAsync(FolderContentDataSourceContext context, CancellationToken token)
    {
        if (string.IsNullOrEmpty(repositoryRequest.RepositoryId))
            throw new PluginMisconfigurationException("Please specify the repository ID first");

        var repoId = long.Parse(repositoryRequest.RepositoryId);
        var branch = branchRequest?.Name;

        var rawPath = context.FolderId;
        string pathForApi = "";

        if (!string.IsNullOrEmpty(rawPath))
        {
            rawPath = rawPath.Replace("\\", "/").TrimEnd('/');
            if (Path.HasExtension(rawPath))
                pathForApi = Path.GetDirectoryName(rawPath)?.Replace("\\", "/") ?? "";
            else
                pathForApi = rawPath;
        }

        var contents = await ExecuteWithErrorHandlingAsync(async () =>
        {
            if (!string.IsNullOrEmpty(branch))
            {
                return string.IsNullOrEmpty(pathForApi)
                    ? await ClientSdk.Repository.Content.GetAllContentsByRef(repoId, branch)
                    : await ClientSdk.Repository.Content.GetAllContentsByRef(repoId, pathForApi, branch);
            }

            return string.IsNullOrEmpty(pathForApi)
                ? await ClientSdk.Repository.Content.GetAllContents(repoId)
                : await ClientSdk.Repository.Content.GetAllContents(repoId, pathForApi);
        });

        var items = contents.Select(x =>
        {
            var isFolder = x.Type == ContentType.Dir;
            return isFolder
                ? (FileDataItem)new Folder
                {
                    Id = x.Path,
                    DisplayName = x.Name,
                    IsSelectable = true
                }
                : new File
                {
                    Id = x.Path,
                    DisplayName = x.Name,
                    IsSelectable = false,
                };
        });

        return items
            .OrderBy(i => i is File)
            .ThenBy(i => i.DisplayName);
    }
}