using Apps.Github.Models.Responses;
using Blackbird.Applications.Sdk.Common.Webhooks;
using Newtonsoft.Json;

namespace Apps.Github;
[WebhookList]
public class Webhooks
{
    [Webhook]
    public Task<WebhookResponse<RepoChangesResponse>> OnRepoChanges(WebhookRequest webhookRequest)
    {
        var result = JsonConvert.DeserializeObject<RepoChanges>(webhookRequest.Body.ToString());

        return Task.FromResult(new WebhookResponse<RepoChangesResponse>
        {
            Result = new RepoChangesResponse
            {
                Name = result.Repository.Name,
                Url = result.Repository.Url
            }
        });
    }
}

