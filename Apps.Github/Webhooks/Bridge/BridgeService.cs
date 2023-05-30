using Blackbird.Applications.Sdk.Common.Authentication;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Github.Webhooks.Bridge
{
    public class BridgeService
    {
        public BridgeService(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders) 
        {

        }
        public void Subscribe(string _event, string repositoryId, string url)
        {
            var client = new RestClient(ApplicationConstants.BridgeServiceUrl);
            var request = new RestRequest($"/{repositoryId}/{_event}", Method.Post);
            request.AddBody(url);
            client.Execute(request);
        }

        public void Unsubscribe(string _event, string repositoryId)
        {
            var client = new RestClient(ApplicationConstants.BridgeServiceUrl);
            var request = new RestRequest($"/{repositoryId}/{_event}", Method.Delete);
            client.Execute(request);
        }
    }
}
