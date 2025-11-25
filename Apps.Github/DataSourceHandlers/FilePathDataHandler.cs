using Apps.GitHub.Models.Branch.Requests;
using Apps.Github.Models.Respository.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using Blackbird.Applications.SDK.Extensions.FileManagement.Models.FileDataSourceItems;

namespace Apps.GitHub.DataSourceHandlers;

public class FilePathDataHandler(
    InvocationContext invocationContext,
    [ActionParameter] GetRepositoryRequest repositoryRequest,
    [ActionParameter] GetOptionalBranchRequest branchRequest)
    : BaseFileDataHandler(invocationContext), IAsyncFileDataSourceItemHandler
{
    public async Task<IEnumerable<FolderPathItem>> GetFolderPathAsync(FolderPathDataSourceContext context, CancellationToken token)
    {
        return GetFolderPath(context.FileDataItemId);
    }

    public async Task<IEnumerable<FileDataItem>> GetFolderContentAsync(FolderContentDataSourceContext context, CancellationToken token)
    {
        if (string.IsNullOrEmpty(repositoryRequest.RepositoryId))
            throw new PluginMisconfigurationException("Please specify the repository ID first");

        var repoId = long.Parse(repositoryRequest.RepositoryId);
        string? branch = branchRequest?.Name;

        string? pathForApi = context.FolderId;
        return await GetFolderContentAsync(repoId, branch, pathForApi, true, false);
    }
}