using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Github.Webhooks.Handlers
{
    public class PushActionHandler : BaseWebhookHandler
    {
        const string SubscriptionEvent = "push";

        public PushActionHandler() : base(SubscriptionEvent) { }
    }
}
