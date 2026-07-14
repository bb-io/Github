using Apps.Github.Webhooks.Handlers;
using Apps.Github.Webhooks.Payloads;
using Blackbird.Applications.Sdk.Common.Webhooks;
using Newtonsoft.Json;
using System.Net;

namespace Apps.Github.Webhooks;

[WebhookList]
public class PullRequestWebhooks
{
    private const string EventNameHeaderKey = "X-GitHub-Event";
    private const string PingEventName = "ping";

    [Webhook("On pull request action", typeof(PullRequestOpenHandler), Description = "Occurs when there is activity on a pull request")]
    public async Task<WebhookResponse<PullRequestPayloadFlat>> PullRequestOpenedHandler(WebhookRequest webhookRequest)
    {
        if (IsPingEvent(webhookRequest))
        {
            return GeneratePreflight<PullRequestPayloadFlat>();
        }

        var data = JsonConvert.DeserializeObject<PullRequestPayload>(webhookRequest.Body.ToString());
        if (data is null)
        {
            throw new InvalidCastException(nameof(webhookRequest.Body));
        }

        return new()
        {
            HttpResponseMessage = null,
            Result = new(data)
        };
    }

    private static bool IsPingEvent(WebhookRequest webhookRequest)
    {
        if (webhookRequest.Headers == null)
        {
            return false;
        }

        var eventHeader = webhookRequest.Headers.FirstOrDefault(h => h.Key.Equals(EventNameHeaderKey, StringComparison.OrdinalIgnoreCase));
        return eventHeader.Value != null && eventHeader.Value.Equals(PingEventName, StringComparison.OrdinalIgnoreCase);
    }

    private static WebhookResponse<T> GeneratePreflight<T>() where T : class
    {
        return new()
        {
            ReceivedWebhookRequestType = WebhookRequestType.Preflight,
            HttpResponseMessage = new(statusCode: HttpStatusCode.OK)
        };
    }
}
