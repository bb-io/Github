using Blackbird.Applications.Sdk.Common;

namespace Apps.Github.Models.PullRequest.Requests
{
    public class MergePullRequest
    {
        [Display("Repository ID")]
        public string RepositoryId { get; set; }

        [Display("Pull request number")]
        public int PullRequestNumber { get; set; }
    }
}
