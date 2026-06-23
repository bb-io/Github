using Newtonsoft.Json;

namespace Apps.GitHub.Models.Workflow;

public class WorkflowRunApiResponse
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("workflow_id")]
    public long WorkflowId { get; set; }

    [JsonProperty("name")]
    public string? Name { get; set; }

    [JsonProperty("display_title")]
    public string? DisplayTitle { get; set; }

    [JsonProperty("event")]
    public string? Event { get; set; }

    [JsonProperty("status")]
    public string? Status { get; set; }

    [JsonProperty("conclusion")]
    public string? Conclusion { get; set; }

    [JsonProperty("head_branch")]
    public string? HeadBranch { get; set; }

    [JsonProperty("head_sha")]
    public string? HeadSha { get; set; }

    [JsonProperty("run_attempt")]
    public int? RunAttempt { get; set; }

    [JsonProperty("url")]
    public string? Url { get; set; }

    [JsonProperty("html_url")]
    public string? HtmlUrl { get; set; }

    [JsonProperty("jobs_url")]
    public string? JobsUrl { get; set; }

    [JsonProperty("logs_url")]
    public string? LogsUrl { get; set; }

    [JsonProperty("created_at")]
    public DateTime? CreatedAt { get; set; }

    [JsonProperty("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [JsonProperty("run_started_at")]
    public DateTime? RunStartedAt { get; set; }
}
