using Blackbird.Applications.Sdk.Common;

namespace Apps.Github.Models.Branch.Requests
{
    public class MergeBranchRequest
    {
        [Display("Repository ID")]
        public string RepositoryId { get; set; }

        [Display("Base branch")]
        public string BaseBranch { get; set; }

        [Display("Head branch")]
        public string HeadBranch { get; set; }

        [Display("Commit message")]
        public string CommitMessage { get; set; }
    }
}
