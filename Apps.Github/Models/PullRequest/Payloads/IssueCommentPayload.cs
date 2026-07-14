using Newtonsoft.Json;

namespace Apps.Github.Models.PullRequest.Payloads;

public class IssueCommentPayload
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("node_id")]
    public string? NodeId { get; set; }

    [JsonProperty("body")]
    public string? Body { get; set; }

    [JsonProperty("html_url")]
    public string? HtmlUrl { get; set; }

    [JsonProperty("user")]
    public GithubUserPayload? User { get; set; }

    [JsonProperty("created_at")]
    public DateTimeOffset? CreatedAt { get; set; }

    [JsonProperty("updated_at")]
    public DateTimeOffset? UpdatedAt { get; set; }
}
