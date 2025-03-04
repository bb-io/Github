using System.Text.Json;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication.OAuth2;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Github.Auth.OAuth2;

public class OAuth2TokenService(InvocationContext invocationContext)
    : BaseInvocable(invocationContext), IOAuth2TokenService
{
    private const string TokenUrl = "https://github.com/login/oauth/access_token";

    public bool IsRefreshToken(Dictionary<string, string> values)
    {
        return false;
    }

    public Task<Dictionary<string, string>> RefreshToken(Dictionary<string, string> values, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<Dictionary<string, string>> RequestToken(string state, string code, Dictionary<string, string> values, CancellationToken cancellationToken)
    {
        const string grantType = "authorization_code";
        var bodyParameters = new Dictionary<string, string>
        {
            { "grant_type", grantType },
            { "client_id", ApplicationConstants.ClientId },
            { "client_secret", ApplicationConstants.ClientSecret },
            { "code", code },
            { "redirect_uri", "https://bridge.blackbird.io/api/AuthorizationCode" }
        };
        
        return await RequestToken(bodyParameters, cancellationToken);
    }

    public Task RevokeToken(Dictionary<string, string> values)
    {
        throw new NotImplementedException();
    }

    private async Task<Dictionary<string, string>> RequestToken(Dictionary<string, string> bodyParameters, CancellationToken cancellationToken)
    {
        using HttpClient httpClient = new();
        httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        using var httpContent = new FormUrlEncodedContent(bodyParameters);
        using var response = await httpClient.PostAsync(TokenUrl, httpContent, cancellationToken);
        var responseContent = await response.Content.ReadAsStringAsync();
        var resultDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(responseContent)?.ToDictionary(r => r.Key, r => r.Value?.ToString())
                               ?? throw new InvalidOperationException($"Invalid response content: {responseContent}");
        
        return resultDictionary!;
    }
}