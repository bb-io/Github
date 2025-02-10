using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.GitHub.DataSourceHandlers;

public class RepositoryAuthorsDataHandler : GithubInvocable, IAsyncDataSourceItemHandler
{
    public RepositoryAuthorsDataHandler(InvocationContext invocationContext) : base(invocationContext){}

    async Task<IEnumerable<DataSourceItem>> IAsyncDataSourceItemHandler.GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(context.SearchString))
        {
                return new List<DataSourceItem>();
        }

        var contributors = await ClientSdk.Repository.GetAllContributors(long.Parse(context.SearchString));

        return contributors.Where(x => context.SearchString == null || x.Login.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
                      .Take(30).Select(x => new DataSourceItem(x.Login, $"{x.Login}"));
    }
}
