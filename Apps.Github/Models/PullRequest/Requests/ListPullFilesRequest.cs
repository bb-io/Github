using Blackbird.Applications.Sdk.Common;

namespace Apps.Github.Models.PullRequest.Requests
{
    public class ListPullFilesRequest
    {
        [Display("Repository ID")]
        public string RepositoryId { get; set; }

        [Display("Pull request number")]
        public string PullRequestNumber { get; set; }
    }
}
