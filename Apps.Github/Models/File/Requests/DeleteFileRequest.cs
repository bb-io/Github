using Blackbird.Applications.Sdk.Common;

namespace Apps.GitHub.Models.File.Requests;

public class DeleteFileRequest : DownloadFileRequest
{
    [Display("Commit message")]
    public string CommitMessage { get; set; }
}