using Blackbird.Applications.Sdk.Common.Authentication;
using Octokit;

namespace Apps.GitHub.Api;

public class GithubOctokitClient : GitHubClient
{
    public GithubOctokitClient(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders) : base(new ProductHeaderValue("Blackbird"))
    {
        var apiToken = authenticationCredentialsProviders.First(p => p.KeyName == "Authorization").Value;
        var tokenAuth = new Credentials(apiToken, AuthenticationType.Bearer);
        Credentials = tokenAuth;
    }
}