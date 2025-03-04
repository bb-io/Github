using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.GitHub.DataSourceHandlers;

public class RepositoryAuthorsDataHandler(InvocationContext invocationContext)
    : GithubInvocable(invocationContext), IAsyncDataSourceItemHandler
{
    async Task<IEnumerable<DataSourceItem>> IAsyncDataSourceItemHandler.GetDataAsync(DataSourceContext context,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(context.SearchString))
        {
            return new List<DataSourceItem>();
        }

        var contributors = await ExecuteWithErrorHandlingAsync(async () =>
            await ClientSdk.Repository.GetAllContributors(long.Parse(context.SearchString)));

        return contributors.Where(x =>
                context.SearchString == null ||
                x.Login.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .Select(x => new DataSourceItem(x.Login, $"{x.Login}"));
    }
}