using Apps.Github.Webhooks.Handlers;
using Apps.Github.Webhooks.Payloads;
using Blackbird.Applications.Sdk.Common.Webhooks;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Text.Json;

namespace Apps.Github.Webhooks
{
    [WebhookList]
    public class PushWebhooks
    {
        [Webhook("On files added", typeof(PushActionHandler), Description = "On files added")]
        public async Task<WebhookResponse<PushPayload>> FilesAddedHandler(WebhookRequest webhookRequest)
        {
            var data = JsonConvert.DeserializeObject<PushPayload>(webhookRequest.Body.ToString());
            if (data is null) { throw new InvalidCastException(nameof(webhookRequest.Body)); }
            return new WebhookResponse<PushPayload>
            {
                HttpResponseMessage = null,
                Result = data
            };
        }

        [Webhook("On files modified", typeof(PushActionHandler), Description = "On files modified")]
        public async Task<WebhookResponse<PushPayload>> FilesModifiedHandler(WebhookRequest webhookRequest)
        {
            var data = JsonConvert.DeserializeObject<PushPayload>(webhookRequest.Body.ToString());
            if (data is null) { throw new InvalidCastException(nameof(webhookRequest.Body)); }
            return new WebhookResponse<PushPayload>
            {
                HttpResponseMessage = null,
                Result = data
            };
        }
    }
}
