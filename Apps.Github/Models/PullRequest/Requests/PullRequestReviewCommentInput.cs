using Newtonsoft.Json;

namespace Apps.Github.Models.PullRequest.Requests;

public class PullRequestReviewCommentInput
{
    [JsonProperty("path")]
    public string? Path { get; set; }

    [JsonProperty("line")]
    public int? Line { get; set; }

    [JsonProperty("side")]
    public string? Side { get; set; }

    [JsonProperty("start_line")]
    public int? StartLine { get; set; }

    [JsonProperty("start_side")]
    public string? StartSide { get; set; }

    [JsonProperty("body")]
    public string? Body { get; set; }
}
