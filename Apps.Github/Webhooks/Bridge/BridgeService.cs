using Apps.Github.Webhooks.Payloads;
using RestSharp;

namespace Apps.Github.Webhooks.Bridge;

public class BridgeService(string bridgeServiceUrl)
{
    private string BridgeServiceUrl { get; set; } = bridgeServiceUrl;

    public async Task Subscribe(string @event, string repositoryId, string url)
    {
        var client = new RestClient(BridgeServiceUrl);
        var request = new RestRequest($"/{repositoryId}/{@event}", Method.Post);
        request.AddHeader("Blackbird-Token", ApplicationConstants.BlackbirdToken);
        request.AddBody(url);
        var response = await client.ExecuteAsync(request);

        if (!response.IsSuccessful)
        {
            throw new InvalidOperationException(
                $"Bridge subscribe failed with status {(int)response.StatusCode}: {response.Content ?? response.ErrorMessage ?? "No response body"}");
        }
    }

    public async Task Unsubscribe(string @event, string repositoryId, string url)
    {
        var client = new RestClient(BridgeServiceUrl);
        var requestGet = new RestRequest($"/{repositoryId}/{@event}", Method.Get);
        requestGet.AddHeader("Blackbird-Token", ApplicationConstants.BlackbirdToken);
        var webhooks = await client.GetAsync<List<BridgeGetResponse>>(requestGet);
        if (webhooks != null)
        {
            var webhook = webhooks.FirstOrDefault(w => w.Value == url);
            if (webhook == null)
            {
                return;
            }

            var requestDelete = new RestRequest($"/{repositoryId}/{@event}/{webhook.Id}", Method.Delete);
            requestDelete.AddHeader("Blackbird-Token", ApplicationConstants.BlackbirdToken);
            var response = await client.DeleteAsync(requestDelete);

            if (!response.IsSuccessful)
            {
                throw new InvalidOperationException(
                    $"Bridge unsubscribe failed with status {(int)response.StatusCode}: {response.Content ?? response.ErrorMessage ?? "No response body"}");
            }
        }
    }
}
