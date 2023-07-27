using Blackbird.Applications.Sdk.Common;

namespace Apps.Github.Models.Branch.Requests
{
    public class ListRepositoryBranchesRequest
    {
        [Display("Repository ID")]
        public string RepositoryId { get; set; }
    }
}
