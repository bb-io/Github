using Blackbird.Applications.Sdk.Common;

namespace Apps.Github.Models.Requests
{
    public class RepositoryContentRequest
    {
        [Display("Repository ID")]
        public string RepositoryId { get; set; }

        public string? Path { get; set; }
    }
}
