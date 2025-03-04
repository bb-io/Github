using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Invocation;
using Apps.Github.Models.Respository.Requests;
using Apps.GitHub.Models.Branch.Requests;
using Octokit;
using Blackbird.Applications.Sdk.Common.Exceptions;

namespace Apps.GitHub.DataSourceHandlers;

public class FilePathDataHandler(
    InvocationContext invocationContext,
    [ActionParameter] GetRepositoryRequest repositoryRequest,
    [ActionParameter] GetOptionalBranchRequest branchRequest)
    : GithubInvocable(invocationContext), IAsyncDataSourceItemHandler
{
    private GetRepositoryRequest RepositoryRequest { get; set; } = repositoryRequest;
    private GetOptionalBranchRequest BranchRequest { get; set; } = branchRequest;

    private const int VisibleFilePathSymbolsNumber = 40;

    async Task<IEnumerable<DataSourceItem>> IAsyncDataSourceItemHandler.GetDataAsync(DataSourceContext context,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(RepositoryRequest.RepositoryId))
        {
            throw new PluginMisconfigurationException("Please, specify repository first");
        }

        var repositoryInfo = await ExecuteWithErrorHandlingAsync(async () =>
            await ClientSdk.Repository.Get(long.Parse(RepositoryRequest.RepositoryId)));

        var tree = await ExecuteWithErrorHandlingAsync(async () =>
            await ClientSdk.Git.Tree.GetRecursive(long.Parse(RepositoryRequest.RepositoryId),
                BranchRequest?.Name ?? repositoryInfo.DefaultBranch));
        return tree.Tree
            .Where(x => x.Type.Value == TreeType.Blob)
            .Where(x => context.SearchString == null ||
                        x.Path.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .Select(x => new DataSourceItem(x.Path,
                x.Path.Length > VisibleFilePathSymbolsNumber ? x.Path[^VisibleFilePathSymbolsNumber..] : x.Path));
    }
}