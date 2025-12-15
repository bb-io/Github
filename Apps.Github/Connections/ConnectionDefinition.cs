using Apps.GitHub.Constants;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Connections;

namespace Apps.Github.Connections;

public class ConnectionDefinition : IConnectionDefinition
{
    public IEnumerable<ConnectionPropertyGroup> ConnectionPropertyGroups => new List<ConnectionPropertyGroup>
    {
        new()
        {
            Name = "OAuth",
            DisplayName = "OAuth",
            AuthenticationType = ConnectionAuthenticationType.OAuth2,
            ConnectionProperties = new List<ConnectionProperty>()
        },
        new()
        {
            Name = "Personal Access Token",
            DisplayName = "Personal Access Token",
            AuthenticationType = ConnectionAuthenticationType.Undefined,
            ConnectionProperties = new List<ConnectionProperty>()
            {
                new (CredNames.PersonalAccessToken) { DisplayName = "Personal Access Token", Sensitive = true}
            }
        }
    };

    public IEnumerable<AuthenticationCredentialsProvider> CreateAuthorizationCredentialsProviders(Dictionary<string, string> values)
    {
        if (values.ContainsKey(CredNames.PersonalAccessToken))
        {
            var pat = values[CredNames.PersonalAccessToken];
            return [new("Authorization", pat)];
        }

        var token = values.First(v => v.Key == "access_token");
        return [new(
            "Authorization",
            $"{token.Value}"
        )];
    }
}