using Newtonsoft.Json;

namespace Apps.Github.Models.PullRequest.Payloads;

public class GithubUserPayload
{
    [JsonProperty("login")]
    public string? Login { get; set; }
}
