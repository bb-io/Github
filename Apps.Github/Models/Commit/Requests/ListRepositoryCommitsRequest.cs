using Blackbird.Applications.Sdk.Common;

namespace Apps.Github.Models.Commit.Requests
{
    public class ListRepositoryCommitsRequest
    {
        [Display("Repository ID")]
        public string RepositoryId { get; set; }
    }
}
