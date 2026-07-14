using Newtonsoft.Json;

namespace Apps.Github.Models.PullRequest.Payloads;

public class PullRequestReviewCommentPayload
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("node_id")]
    public string? NodeId { get; set; }

    [JsonProperty("body")]
    public string? Body { get; set; }

    [JsonProperty("path")]
    public string? Path { get; set; }

    [JsonProperty("diff_hunk")]
    public string? DiffHunk { get; set; }

    [JsonProperty("line")]
    public int? Line { get; set; }

    [JsonProperty("start_line")]
    public int? StartLine { get; set; }

    [JsonProperty("side")]
    public string? Side { get; set; }

    [JsonProperty("start_side")]
    public string? StartSide { get; set; }

    [JsonProperty("commit_id")]
    public string? CommitId { get; set; }

    [JsonProperty("original_commit_id")]
    public string? OriginalCommitId { get; set; }

    [JsonProperty("pull_request_review_id")]
    public long? PullRequestReviewId { get; set; }

    [JsonProperty("html_url")]
    public string? HtmlUrl { get; set; }

    [JsonProperty("user")]
    public GithubUserPayload? User { get; set; }

    [JsonProperty("created_at")]
    public DateTimeOffset? CreatedAt { get; set; }

    [JsonProperty("updated_at")]
    public DateTimeOffset? UpdatedAt { get; set; }
}
