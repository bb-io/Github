using Apps.Github.Webhooks.Handlers;
using Apps.Github.Webhooks.Payloads;
using Blackbird.Applications.Sdk.Common.Webhooks;
using Newtonsoft.Json;

namespace Apps.Github.Webhooks;

[WebhookList]
public class PullRequestWebhooks
{
    [Webhook("On pull request action", typeof(PullRequestOpenHandler), Description = "On pull request action")]
    public async Task<WebhookResponse<PullRequestPayloadFlat>> PullRequestOpenedHandler(WebhookRequest webhookRequest)
    {
        var data = JsonConvert.DeserializeObject<PullRequestPayload>(webhookRequest.Body.ToString());
        if (data is null) { throw new InvalidCastException(nameof(webhookRequest.Body)); }
        return new WebhookResponse<PullRequestPayloadFlat>
        {
            HttpResponseMessage = null,
            Result = new PullRequestPayloadFlat(data)
        };
    }
}