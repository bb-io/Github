using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication.OAuth2;
using Blackbird.Applications.Sdk.Common.Invocation;
using Microsoft.AspNetCore.WebUtilities;

namespace Apps.Github.Auth.OAuth2;

public class OAuth2AuthorizeService : IOAuth2AuthorizeService
{
    public OAuth2AuthorizeService()
    {
    }

    public string GetAuthorizationUrl(Dictionary<string, string> values)
    {
        string bridgeOauthUrl = $"{"https://bridge.blackbird.io/api"}/oauth";
        const string oauthUrl = "https://github.com/login/oauth/authorize";
        var parameters = new Dictionary<string, string>
        {
            { "client_id", ApplicationConstants.ClientId },
            { "redirect_uri", $"{"https://bridge.blackbird.io/api"}/AuthorizationCode" },
            { "scope", ApplicationConstants.Scope },
            { "state", values["state"] },
            { "authorization_url", oauthUrl},
            { "actual_redirect_uri", "https://sandbox.blackbird.io/api-rest/connections/AuthorizationCode" },
        };
        return QueryHelpers.AddQueryString(bridgeOauthUrl, parameters);
    }
}