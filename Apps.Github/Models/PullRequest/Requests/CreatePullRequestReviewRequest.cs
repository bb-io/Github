using Blackbird.Applications.Sdk.Common;

namespace Apps.Github.Models.PullRequest.Requests;

public class CreatePullRequestReviewRequest
{
    [Display("Commit ID")]
    public string CommitId { get; set; } = string.Empty;

    public string? Body { get; set; }

    [Display("Review event", Description = "Use COMMENT, APPROVE, or REQUEST_CHANGES. Defaults to COMMENT.")]
    public string? Event { get; set; }

    [Display("Review comments JSON", Description = "JSON array of inline comments. Each item should include path, line, side, and body.")]
    public string? CommentsJson { get; set; }
}
