using Apps.GitHub.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Github.Models.Respository.Requests;

public class DownloadFileRequest
{
    // Add a dynamic dropdown of tree + filter type == blob && name
    [Display("File path")]
    [DataSource(typeof(FilePathDataHandler))]
    public string FilePath { get; set; }
}