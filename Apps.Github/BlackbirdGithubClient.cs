using Blackbird.Applications.Sdk.Common.Authentication;
using Octokit;

namespace Apps.Github
{
    public class BlackbirdGithubClient : GitHubClient
    {
        public BlackbirdGithubClient(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders) : base (new ProductHeaderValue("Blackbird"))
        {
            var apiToken = authenticationCredentialsProviders.First(p => p.KeyName == "Authorization").Value;
            var tokenAuth = new Credentials(apiToken, AuthenticationType.Bearer);
            Credentials = tokenAuth;
        }
    }
}
