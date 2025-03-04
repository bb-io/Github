using Apps.GitHub;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Github.DataSourceHandlers;

public class UsersDataHandler(InvocationContext invocationContext)
    : GithubInvocable(invocationContext), IAsyncDataSourceItemHandler
{
    async Task<IEnumerable<DataSourceItem>> IAsyncDataSourceItemHandler.GetDataAsync(DataSourceContext context,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(context.SearchString))
        {
            return new List<DataSourceItem>();
        }

        var content =
            await ExecuteWithErrorHandlingAsync(async () =>
                await ClientSdk.Search.SearchUsers(new(context.SearchString)));
        return content.Items.Take(30).Select(x => new DataSourceItem(x.Login, $"{x.Login}"));
    }
}