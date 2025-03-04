using Apps.Github.Webhooks.Payloads;
using Blackbird.Applications.Sdk.Common.Authentication;
using RestSharp;

namespace Apps.Github.Webhooks.Bridge;

public class BridgeService(string bridgeServiceUrl)
{
    private string BridgeServiceUrl { get; set; } = bridgeServiceUrl;

    public void Subscribe(string @event, string repositoryId, string url)
    {
        var client = new RestClient(BridgeServiceUrl);
        var request = new RestRequest($"/{repositoryId}/{@event}", Method.Post);
        request.AddHeader("Blackbird-Token", ApplicationConstants.BlackbirdToken);
        request.AddBody(url);
        client.Execute(request);
    }

    public void Unsubscribe(string @event, string repositoryId, string url)
    {
        var client = new RestClient(BridgeServiceUrl);
        var requestGet = new RestRequest($"/{repositoryId}/{@event}", Method.Get);
        requestGet.AddHeader("Blackbird-Token", ApplicationConstants.BlackbirdToken);
        var webhooks = client.Get<List<BridgeGetResponse>>(requestGet);
        if (webhooks != null)
        {
            var webhook = webhooks.FirstOrDefault(w => w.Value == url);

            var requestDelete = new RestRequest($"/{repositoryId}/{@event}/{webhook.Id}", Method.Delete);
            requestDelete.AddHeader("Blackbird-Token", ApplicationConstants.BlackbirdToken);
            client.Delete(requestDelete);
        }
    }
}