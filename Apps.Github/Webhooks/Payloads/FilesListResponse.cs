using Blackbird.Applications.Sdk.Common;

namespace Apps.Github.Webhooks.Payloads
{
    public class FilesListResponse
    {
        public IEnumerable<FilePathObj> Files { get; set; }
    }

    public class FilePathObj
    {

        [Display("File path")]
        public string FilePath { get; set; }
    }
}
