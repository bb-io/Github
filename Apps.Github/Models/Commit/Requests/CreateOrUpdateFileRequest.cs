using Apps.GitHub.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.Github.Models.Commit.Requests;

public class CreateOrUpdateFileRequest
{
    [Display("Folder")]
    [DataSource(typeof(FolderPathDataHandler))]
    public string Folder { get; set; }

    [Display("File")]
    public FileReference File { get; set; }

    [Display("Commit message")]
    public string CommitMessage { get; set; }
}