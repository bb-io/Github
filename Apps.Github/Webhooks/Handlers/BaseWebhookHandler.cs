using Apps.Github.Webhooks.Bridge;
using Apps.Github.Webhooks.Payloads;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Webhooks;

namespace Apps.Github.Webhooks.Handlers;

public class BaseWebhookHandler(InvocationContext invocationContext, WebhookRepositoryInput input, string subEvent)
    : BaseInvocable(invocationContext), IWebhookEventHandler
{
    private string SubscriptionEvent { get; set; } = subEvent;

    private string RepositoryId { get; set; } = input.RepositoryId;

    public async Task SubscribeAsync(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders, 
        Dictionary<string, string> values)
    {
        var bridge = new BridgeService($"{InvocationContext.UriInfo.BridgeServiceUrl.ToString().TrimEnd('/')}/webhooks/github");
        bridge.Subscribe(SubscriptionEvent, RepositoryId, values["payloadUrl"]);
    }

    public async Task UnsubscribeAsync(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders, 
        Dictionary<string, string> values)
    {
        var bridge = new BridgeService($"{InvocationContext.UriInfo.BridgeServiceUrl.ToString().TrimEnd('/')}/webhooks/github");
        bridge.Unsubscribe(SubscriptionEvent, RepositoryId, values["payloadUrl"]);
    }
}