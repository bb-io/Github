using Apps.GitHub.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.SDK.Extensions.FileManagement.Models.FileDataSourceItems;

namespace Apps.GitHub.Models.File.Requests;

public class DownloadFileRequest
{
    [Display("File path")]
    [FileDataSource(typeof(FilePathDataHandler))]
    public string FilePath { get; set; }
}