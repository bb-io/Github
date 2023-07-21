using Apps.Github.Webhooks.Payloads;
using Blackbird.Applications.Sdk.Common;

namespace Apps.Github.Models.Respository.Requests
{
    public class GetRepositoryFilesFromFilepathsRequest
    {
        [Display("Repository ID")]
        public string RepositoryId { get; set; }


        [Display("Filepaths array (only from webhooks)")]
        public IEnumerable<FilePathObj> Files { get; set; }
    }
}
