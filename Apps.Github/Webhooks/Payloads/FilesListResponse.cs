using Blackbird.Applications.Sdk.Common;
using Octokit;

namespace Apps.Github.Webhooks.Payloads;

public class FilesListResponse
{
    [Display("File paths")]
    public IEnumerable<FilePathObj> Files { get; set; }
}

public class FilePathObj
{

    [Display("File path")]
    public string FilePath { get; set; }
}