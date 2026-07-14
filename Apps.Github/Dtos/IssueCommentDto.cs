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
    public DateTimeOffset? CreatedAt { get; set; } = source.CreatedAt;

    [Display("Updated at")]
    public DateTimeOffset? UpdatedAt { get; set; } = source.UpdatedAt;
}
