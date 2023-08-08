using Blackbird.Applications.Sdk.Common.Authentication.OAuth2;
using Microsoft.AspNetCore.WebUtilities;

namespace Apps.Github.Auth.OAuth2
{
    public class OAuth2AuthorizeService : IOAuth2AuthorizeService
    {
        public string GetAuthorizationUrl(Dictionary<string, string> values)
        {
            const string oauthUrl = "https://github.com/login/oauth/authorize";
            var parameters = new Dictionary<string, string>
            {
                { "client_id", ApplicationConstants.ClientId },
                { "redirect_uri", ApplicationConstants.RedirectUri },
                { "scope", ApplicationConstants.Scope },
                { "state", values["state"] }
            };
            return QueryHelpers.AddQueryString(oauthUrl, parameters);
        }
    }
}
