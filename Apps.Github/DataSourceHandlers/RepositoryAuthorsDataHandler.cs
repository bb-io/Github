using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Invocation;
using Apps.Github.Models.Respository.Requests;
using Apps.Github.DataSourceHandlers;
using Apps.Github.Actions.Base;

namespace Apps.GitHub.DataSourceHandlers;

public class RepositoryAuthorsDataHandler : GithubActions, IAsyncDataSourceHandler
{
    public GetRepositoryRequest RepositoryRequest { get; set; }

    private IEnumerable<AuthenticationCredentialsProvider> Creds =>
    InvocationContext.AuthenticationCredentialsProviders;

    public RepositoryAuthorsDataHandler(InvocationContext invocationContext, 
        [ActionParameter] GetRepositoryRequest repositoryRequest) : base(invocationContext)
    {
        RepositoryRequest = repositoryRequest;
    }

    public async Task<Dictionary<string, string>> GetDataAsync(
        DataSourceContext context,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(RepositoryRequest.RepositoryId))
            return await new UsersDataHandler(InvocationContext).GetDataAsync(context, cancellationToken);

        var content = await ClientSdk.Repository.GetAllContributors(long.Parse(RepositoryRequest.RepositoryId));
        return content.Where(x => context.SearchString == null ||
                        x.Login.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .Take(30).ToDictionary(x => x.Login, x => $"{x.Login}");
    }
}
