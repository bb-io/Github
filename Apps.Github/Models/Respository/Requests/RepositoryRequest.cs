using Blackbird.Applications.Sdk.Common;

namespace Apps.Github.Models.Requests
{
    public class RepositoryRequest
    {
        [Display("Repository ID")]
        public string RepositoryId { get; set; }
    }
}
