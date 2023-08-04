using Blackbird.Applications.Sdk.Common;

namespace Apps.Github.Models.Branch.Requests
{
    public class GetBranchRequest
    {
        [Display("Repository ID")]
        public string RepositoryId { get; set; }
        public string Name { get; set; }
    }
}
