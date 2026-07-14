using Blackbird.Applications.Sdk.Common;

namespace Apps.Github.Models.PullRequest.Requests;

public class CommentIdentifierRequest
{
    [Display("Comment ID")]
    public string CommentId { get; set; } = string.Empty;
}
