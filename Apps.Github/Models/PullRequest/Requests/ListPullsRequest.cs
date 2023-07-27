using Blackbird.Applications.Sdk.Common;

namespace Apps.Github.Models.PullRequest.Requests
{
    public class ListPullsRequest
    {
        [Display("Repository ID")]
        public string RepositoryId { get; set; }
    }
}
