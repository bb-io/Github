using Apps.Github.Webhooks.Payloads;
using Blackbird.Applications.Sdk.Common.Webhooks;

namespace Apps.Github.Webhooks.Handlers
{
    public class PullRequestOpenHandler : BaseWebhookHandler
    {
        const string SubscriptionEvent = "pull_request";

        public PullRequestOpenHandler([WebhookParameter] WebhookInput input) : base(SubscriptionEvent, input) { }
    }
}
