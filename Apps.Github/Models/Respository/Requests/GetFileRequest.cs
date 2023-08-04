using Blackbird.Applications.Sdk.Common;

namespace Apps.Github.Models.Requests
{
    public class GetFileRequest
    {
        [Display("Repository ID")]
        public string RepositoryId { get; set; }

        [Display("File path")]
        public string FilePath { get; set; }
    }
}
