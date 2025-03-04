using Apps.GitHub;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Github.DataSourceHandlers;

public class RepositoryDataHandler(InvocationContext invocationContext)
    : GithubInvocable(invocationContext), IAsyncDataSourceItemHandler
{
    async Task<IEnumerable<DataSourceItem>> IAsyncDataSourceItemHandler.GetDataAsync(DataSourceContext context,
        CancellationToken cancellationToken)
    {
        var content = await ExecuteWithErrorHandlingAsync(async () =>
            await ClientSdk.Repository.GetAllForCurrent(new Octokit.ApiOptions { PageSize = 100 }));

        return content
            .Where(x => context.SearchString == null ||
                        x.Name.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new DataSourceItem(x.Id.ToString(), x.Name));
    }
}