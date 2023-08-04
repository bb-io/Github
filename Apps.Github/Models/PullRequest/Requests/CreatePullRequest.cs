using Blackbird.Applications.Sdk.Common;

namespace Apps.Github.Models.PullRequest.Requests
{
    public class CreatePullRequest
    {
        [Display("Repository ID")]
        public string RepositoryId { get; set; }

        public string Title { get; set; }

        [Display("Head branch")]
        public string HeadBranch { get; set; }

        [Display("Base branch")]
        public string BaseBranch { get; set; }
    }
}
