using Blackbird.Applications.Sdk.Common.Authentication;
using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
