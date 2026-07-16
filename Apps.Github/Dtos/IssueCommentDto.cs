using Apps.Github.Models.PullRequest.Payloads;
using Blackbird.Applications.Sdk.Common;

namespace Apps.Github.Dtos;

public class IssueCommentDto(IssueCommentPayload source)
{
    public string Id { get; set; } = source.Id.ToString();

    public string NodeId { get; set; } = source.NodeId ?? string.Empty;

    public string Body { get; set; } = source.Body ?? string.Empty;

    [Display("HTML URL")]
    public string HtmlUrl { get; set; } = source.HtmlUrl ?? string.Empty;

    public string UserLogin { get; set; } = source.User?.Login ?? string.Empty;

    [Display("Created at")]
    public DateTime? CreatedAt { get; set; } = source.CreatedAt?.UtcDateTime;

    [Display("Updated at")]
    public DateTime? UpdatedAt { get; set; } = source.UpdatedAt?.UtcDateTime;
}
