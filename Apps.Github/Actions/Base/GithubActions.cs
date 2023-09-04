using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Github.Actions.Base;

public class GithubActions : BaseInvocable
{
    protected IEnumerable<AuthenticationCredentialsProvider> Creds =>
        InvocationContext.AuthenticationCredentialsProviders;
    
    protected BlackbirdGithubClient Client { get; }
    
    public GithubActions(InvocationContext invocationContext) : base(invocationContext)
    {
        Client = new(Creds);
    }
}