using Blackbird.Applications.Sdk.Common;

namespace Apps.Github.Models.Commit.Requests;

public class UpdateFileRequest : PushFileRequest
{
    [Display("File ID (Sha)")]
    public string? FileId { get; set; }
}