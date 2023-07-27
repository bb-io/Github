using Blackbird.Applications.Sdk.Common;

namespace Apps.Github.Models.Respository.Requests
{
    public class GetRepositoryByIdRequest
    {
        [Display("Repository ID")]
        public string RepositoryId { get; set; }
    }
}
