using Newtonsoft.Json;

namespace Apps.Github.Models.PullRequest.Payloads;

public class PullRequestReviewPayload
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("node_id")]
    public string? NodeId { get; set; }

    [JsonProperty("body")]
    public string? Body { get; set; }

    [JsonProperty("state")]
    public string? State { get; set; }

    [JsonProperty("commit_id")]
    public string? CommitId { get; set; }

    [JsonProperty("html_url")]
    public string? HtmlUrl { get; set; }

    [JsonProperty("user")]
    public GithubUserPayload? User { get; set; }

    [JsonProperty("submitted_at")]
    public DateTimeOffset? SubmittedAt { get; set; }
}
