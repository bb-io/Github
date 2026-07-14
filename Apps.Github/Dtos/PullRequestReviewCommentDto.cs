using Apps.Github.Models.PullRequest.Payloads;
using Blackbird.Applications.Sdk.Common;

namespace Apps.Github.Dtos;

public class PullRequestReviewCommentDto(PullRequestReviewCommentPayload source)
{
    public string Id { get; set; } = source.Id.ToString();

    public string NodeId { get; set; } = source.NodeId ?? string.Empty;

    public string Body { get; set; } = source.Body ?? string.Empty;

    public string Path { get; set; } = source.Path ?? string.Empty;

    [Display("Diff hunk")]
    public string DiffHunk { get; set; } = source.DiffHunk ?? string.Empty;

    public int? Line { get; set; } = source.Line;

    [Display("Start line")]
    public int? StartLine { get; set; } = source.StartLine;

    public string Side { get; set; } = source.Side ?? string.Empty;

    [Display("Start side")]
    public string StartSide { get; set; } = source.StartSide ?? string.Empty;

    [Display("Commit ID")]
    public string CommitId { get; set; } = source.CommitId ?? string.Empty;

    [Display("Original commit ID")]
    public string OriginalCommitId { get; set; } = source.OriginalCommitId ?? string.Empty;

    [Display("Pull request review ID")]
    public string PullRequestReviewId { get; set; } = source.PullRequestReviewId?.ToString() ?? string.Empty;

    [Display("HTML URL")]
    public string HtmlUrl { get; set; } = source.HtmlUrl ?? string.Empty;

    public string UserLogin { get; set; } = source.User?.Login ?? string.Empty;

    [Display("Created at")]
    public DateTimeOffset? CreatedAt { get; set; } = source.CreatedAt;

    [Display("Updated at")]
    public DateTimeOffset? UpdatedAt { get; set; } = source.UpdatedAt;
}
