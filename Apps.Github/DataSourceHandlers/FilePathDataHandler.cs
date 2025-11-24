using Octokit;
using Apps.GitHub.Models.Branch.Requests;
using Apps.Github.Models.Respository.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using Blackbird.Applications.SDK.Extensions.FileManagement.Models.FileDataSourceItems;
using File = Blackbird.Applications.SDK.Extensions.FileManagement.Models.FileDataSourceItems.File;

namespace Apps.GitHub.DataSourceHandlers;

public class FilePathDataHandler(
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
                foreach (var part in parts)
                {
                    breadcrumb.Add(new FolderPathItem
                    {
                        Id = part,
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

        var repositoryInfo = await ExecuteWithErrorHandlingAsync(async () =>
            await ClientSdk.Repository.Get(long.Parse(repositoryRequest.RepositoryId))
        );

        var tree = await ExecuteWithErrorHandlingAsync(async () =>
            await ClientSdk.Git.Tree.GetRecursive(
                long.Parse(repositoryRequest.RepositoryId),
                branchRequest?.Name ?? repositoryInfo.DefaultBranch
            )
        );

        var blobs = tree.Tree.Where(x => x.Type.Value == TreeType.Blob);

        var files = blobs
            .Select(x => new File
            {
                Id = x.Path,
                DisplayName = Path.GetFileName(x.Path),
                IsSelectable = true,
            })
            .ToList();

        var folderPaths = blobs
            .SelectMany(x =>
            {
                var parts = x.Path.Split('/');
                return Enumerable.Range(1, parts.Length - 1).Select(i => string.Join('/', parts.Take(i)));
            })
            .Distinct();

        var folders = folderPaths
            .Select(path => new Folder
            {
                Id = path,
                DisplayName = path.Split('/').Last(),
                IsSelectable = false,
            })
            .ToList();

        var allItems = files.Cast<FileDataItem>()
                            .Concat(folders)
                            .ToList();

        string? folderToDisplay;
        var folderId = context.FolderId;

        if (!string.IsNullOrEmpty(folderId))
        {
            folderId = folderId.TrimEnd('/');

            folderToDisplay = Path.HasExtension(folderId)
                ? Path.GetDirectoryName(folderId)?.Replace("\\", "/") ?? ""
                : folderId;
        }
        else
            folderToDisplay = "";

        var filtered = allItems.Where(i =>
        {
            var parent = Path.GetDirectoryName(i.Id)?.Replace("\\", "/") ?? "";

            if (!string.IsNullOrEmpty(folderToDisplay) && i.Id.StartsWith(folderToDisplay + "/"))
                i.DisplayName = i.Id.Substring(folderToDisplay.Length + 1);
            else
                i.DisplayName = i.Id;

            return parent == folderToDisplay;
        });

        return filtered
            .OrderBy(i => i is File)
            .ThenBy(i => i.Id);
    }
}