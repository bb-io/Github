using Apps.GitHub.Dtos;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Utils.RestSharp;
using Newtonsoft.Json;
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

    public GithubRestClient(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders, RestClientOptions restClientOptions)
        : base(restClientOptions)
    {
        this.AddDefaultHeader("Authorization", GetAcessTokenKey(authenticationCredentialsProviders));
    }

    protected override Exception ConfigureErrorException(RestResponse response)
    {
        var error = JsonConvert.DeserializeObject<RestErrorDto>(response.Content);
        return new GithubErrorException(int.Parse(error.Status), error.Message);
    }

    private static string GetAcessTokenKey(IEnumerable<AuthenticationCredentialsProvider> creds)
        => $"Bearer {creds.First(x => x.KeyName == "Authorization").Value}";
}
