using Apps.GitHub;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Github.DataSourceHandlers;

public class RepositoryDataHandler : GithubInvocable, IAsyncDataSourceHandler
{
    private IEnumerable<AuthenticationCredentialsProvider> Creds =>
        InvocationContext.AuthenticationCredentialsProviders;

    public RepositoryDataHandler(InvocationContext invocationContext) : base(invocationContext)
    {
    }

    public async Task<Dictionary<string, string>> GetDataAsync(
        DataSourceContext context,
        CancellationToken cancellationToken)
    {
        var content = await ClientSdk.Repository.GetAllForCurrent(new Octokit.ApiOptions { PageSize = 100 });

        return content
            .Where(x => context.SearchString == null ||
                        x.Name.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .Take(20)
            .OrderByDescending(x => x.CreatedAt)
            .ToDictionary(x => x.Id.ToString(), x => x.Name);
    }
}