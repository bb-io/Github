using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Utils.RestSharp;
using RestSharp;

namespace Apps.GitHub.Api;

public class GithubRestClient : BlackBirdRestClient
{
    public GithubRestClient(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders)
        : base(
            new RestClientOptions { ThrowOnAnyError = true, BaseUrl = new Uri("https://api.github.com/repos") }
        )
    {
        this.AddDefaultHeader("Authorization", GetAcessTokenKey(authenticationCredentialsProviders));
    }

    protected override Exception ConfigureErrorException(RestResponse response)
    {
        throw new NotImplementedException();
    }

    private static string GetAcessTokenKey(IEnumerable<AuthenticationCredentialsProvider> creds)
        => $"Bearer {creds.First(x => x.KeyName == "access_token").Value}";
}
