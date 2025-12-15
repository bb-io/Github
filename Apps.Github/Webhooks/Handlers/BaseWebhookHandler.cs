using Apps.Github.Webhooks.Bridge;
using Apps.Github.Webhooks.Payloads;
using Apps.GitHub;
using Apps.GitHub.Api;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Webhooks;
using Octokit;

namespace Apps.Github.Webhooks.Handlers;

public class BaseWebhookHandler(InvocationContext invocationContext, WebhookRepositoryInput input, string subEvent)
    : GithubInvocable(invocationContext), IWebhookEventHandler
{
    private string SubscriptionEvent { get; set; } = subEvent;

    private string RepositoryId { get; set; } = input.RepositoryId;

    public async Task SubscribeAsync(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders, 
        Dictionary<string, string> values)
    {
        if (!IsUsingPersonalAccessToken)
        {
            var bridge = new BridgeService($"{InvocationContext.UriInfo.BridgeServiceUrl.ToString().TrimEnd('/')}/webhooks/github");
            bridge.Subscribe(SubscriptionEvent, RepositoryId, values["payloadUrl"]);
            return;
        }

        var hookConfig = new Dictionary<string, string>
        {
            { "url", values["payloadUrl"] },
            { "content_type", "json" },
            { "secret", "myWebhookSecret" },
            { "insecure_ssl", "0" }
        };

        try
        {
            await ClientSdk.Repository.Hooks.Create(long.Parse(input.RepositoryId), new NewRepositoryHook(name: "web", hookConfig) { Events = [ subEvent ], Active = true });
        }
        catch (ApiValidationException ex)
        {
            throw new PluginApplicationException(ex.ApiError?.Message);           
        }
    }

    public async Task UnsubscribeAsync(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders, 
        Dictionary<string, string> values)
    {
        if (!IsUsingPersonalAccessToken)
        {
            var bridge = new BridgeService($"{InvocationContext.UriInfo.BridgeServiceUrl.ToString().TrimEnd('/')}/webhooks/github");
            bridge.Unsubscribe(SubscriptionEvent, RepositoryId, values["payloadUrl"]);
            return;
        }

        var allHooks = await GetAllHooks();
        var thisHook = allHooks.FirstOrDefault(x => x.Config.ContainsKey("url") && x.Config["url"] == values["payloadUrl"]);

        if (thisHook is not null)
        {
            await ClientSdk.Repository.Hooks.Delete(long.Parse(input.RepositoryId), thisHook.Id);
        }        
    }

    public async Task<IReadOnlyList<RepositoryHook>> GetAllHooks()
    {
        var info = await GetRepositoryInfo();
        return await ClientSdk.Repository.Hooks.GetAll(info.Owner.Login, info.Name);
    }

    private async Task<Octokit.Repository> GetRepositoryInfo()
    {
        return await ExecuteWithErrorHandlingAsync(async () => await ClientSdk.Repository.Get(long.Parse(input.RepositoryId)));
    }
}