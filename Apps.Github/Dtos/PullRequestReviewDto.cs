using Blackbird.Applications.Sdk.Common;

namespace Apps.Github.Dtos;

public class PullRequestReviewDto
{
    public string Id { get; set; } = string.Empty;

    public string NodeId { get; set; } = string.Empty;

    public string Body { get; set; } = string.Empty;

    public string State { get; set; } = string.Empty;

    [Display("Commit ID")]
    public string CommitId { get; set; } = string.Empty;

    [Display("HTML URL")]
    public string HtmlUrl { get; set; } = string.Empty;

    public string UserLogin { get; set; } = string.Empty;

    [Display("Submitted at")]
    public DateTime? SubmittedAt { get; set; }
}
