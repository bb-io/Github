using Apps.GitHub.Api;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Octokit;

namespace Apps.GitHub;

public class GithubInvocable : BaseInvocable
{
    public IEnumerable<AuthenticationCredentialsProvider> Creds =>
        InvocationContext.AuthenticationCredentialsProviders;

    protected GithubOctokitClient ClientSdk { get; }
    protected GithubRestClient ClientRest { get; }

    public GithubInvocable(InvocationContext invocationContext) : base(invocationContext)
    {
        ClientSdk = new(Creds);
        ClientRest = new(Creds);
    }

    protected bool IsUsingPersonalAccessToken => Creds.First(p => p.KeyName == "Authorization").Value.StartsWith("github_pat");

    protected async Task ExecuteWithErrorHandlingAsync(Func<Task> action)
    {
        try
        {
            await action();
        }
        catch (Exception ex)
        {
            throw new PluginApplicationException(ex.Message);
        }
    }

    protected async Task<T> ExecuteWithErrorHandlingAsync<T>(Func<Task<T>> action)
    {
        try
        {
            return await action();
        }
        catch (Exception ex)
        {
            throw new PluginApplicationException(ex.Message);
        }
    }
}