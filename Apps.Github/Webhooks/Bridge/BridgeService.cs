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
            //var client = new RestClient();
            //var detailsRequest = new RestRequest("/account-info/v3/details", Method.Get, authenticationCredentialsProviders);
            //var details = client.Get<Details>(detailsRequest);

            //if (details == null) throw new Exception("Could not fetch account details");
            //PortalId = details.PortalId;

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
