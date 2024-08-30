using Apps.GitHub.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.GitHub.Models.File.Requests;

public class DownloadFileRequest
{
    [Display("File path")]
    [DataSource(typeof(FilePathDataHandler))]
    public string FilePath { get; set; }
}