using Apps.Github.Webhooks.Payloads;
using Blackbird.Applications.Sdk.Common.Webhooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Github.Webhooks.Handlers
{
    public class PullRequestOpenHandler : BaseWebhookHandler
    {
        const string SubscriptionEvent = "pull_request";

        public PullRequestOpenHandler([WebhookParameter] WebhookInput input) : base(SubscriptionEvent, input) { }
    }
}
