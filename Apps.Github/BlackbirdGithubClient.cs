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
            var apiToken = authenticationCredentialsProviders.First(p => p.KeyName == "apiToken").Value;
            var tokenAuth = new Credentials(apiToken);
            Credentials = tokenAuth;
        }
    }
}
