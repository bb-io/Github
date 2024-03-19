using Apps.Github.Webhooks.Bridge;
using Apps.Github.Webhooks.Payloads;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Webhooks;

namespace Apps.Github.Webhooks.Handlers;

public class BaseWebhookHandler : BaseInvocable, IWebhookEventHandler
{
    private string SubscriptionEvent { get; set; }

    private string RepositoryId { get; set; }

    public BaseWebhookHandler(InvocationContext invocationContext, WebhookRepositoryInput input, string subEvent) : base(invocationContext)
    {
        SubscriptionEvent = subEvent;
        RepositoryId = input.RepositoryId;
    }

    public async Task SubscribeAsync(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders, 
        Dictionary<string, string> values)
    {
        var bridge = new BridgeService(authenticationCredentialsProviders, $"{InvocationContext.UriInfo.BridgeServiceUrl.ToString().TrimEnd('/')}/webhooks/github");
        bridge.Subscribe(SubscriptionEvent, RepositoryId, values["payloadUrl"]);
    }

    public async Task UnsubscribeAsync(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders, 
        Dictionary<string, string> values)
    {
        var bridge = new BridgeService(authenticationCredentialsProviders, $"{InvocationContext.UriInfo.BridgeServiceUrl.ToString().TrimEnd('/')}/webhooks/github");
        bridge.Unsubscribe(SubscriptionEvent, RepositoryId, values["payloadUrl"]);
    }
}