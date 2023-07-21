using Apps.Github.Webhooks.Payloads;
using Blackbird.Applications.Sdk.Common.Webhooks;

namespace Apps.Github.Webhooks.Handlers
{
    public class PushActionHandler : BaseWebhookHandler
    {
        const string SubscriptionEvent = "push";

        public PushActionHandler([WebhookParameter] WebhookInput input) : base(SubscriptionEvent, input) { }
    }
}
 