using Blackbird.Applications.Sdk.Common;

namespace Apps.Github.Models.Commit.Requests
{
    public class GetCommitRequest
    {
        [Display("Repository ID")]
        public string RepositoryId { get; set; }

        [Display("Commit ID")]
        public string CommitId { get; set; }
    }
}
