using Apps.GitHub.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.SDK.Extensions.FileManagement.Models.FileDataSourceItems;

namespace Apps.Github.Models.Commit.Requests;

public class CreateOrUpdateFileRequest
{
    [Display("File path")]
    [FileDataSource(typeof(FolderPathDataHandler))]
    public string? FilePath { get; set; }

    [Display("File")]
    public FileReference File { get; set; }

    [Display("Commit message")]
    public string CommitMessage { get; set; }
}