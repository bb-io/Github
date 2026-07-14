using Apps.Github.Models.PullRequest.Payloads;
using Blackbird.Applications.Sdk.Common;

namespace Apps.Github.Dtos;

public class PullRequestReviewDto(PullRequestReviewPayload source)
{
    public string Id { get; set; } = source.Id.ToString();

    public string NodeId { get; set; } = source.NodeId ?? string.Empty;

    public string Body { get; set; } = source.Body ?? string.Empty;

    public string State { get; set; } = source.State ?? string.Empty;

    [Display("Commit ID")]
    public string CommitId { get; set; } = source.CommitId ?? string.Empty;

    [Display("HTML URL")]
    public string HtmlUrl { get; set; } = source.HtmlUrl ?? string.Empty;

    public string UserLogin { get; set; } = source.User?.Login ?? string.Empty;

    [Display("Submitted at")]
    public DateTimeOffset? SubmittedAt { get; set; } = source.SubmittedAt;
}
