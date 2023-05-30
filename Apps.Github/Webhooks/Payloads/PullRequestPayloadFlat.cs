using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Github.Webhooks.Payloads
{
    public class PullRequestPayloadFlat
    {
        public PullRequestPayloadFlat(PullRequestPayload source) { 
            Action = source.Action;
            Number = source.Number;
            PullRequestUrl = source.PullRequest.Url;
            PullRequestId = source.PullRequest.Id;
            RepositoryId = source.Repository.Id;
            RepositoryName = source.Repository.Name;
            SenderLogin = source.Sender.Login;
            SenderId = source.Sender.Id;
        }

        public string Action { get; set; }
        public int Number { get; set; }
        public string PullRequestUrl { get; set; }
        public int PullRequestId { get; set; }
        public int RepositoryId { get; set; }
        public string RepositoryName { get; set; }
        public string SenderLogin { get; set; }
        public int SenderId { get; set; }
    }
}
