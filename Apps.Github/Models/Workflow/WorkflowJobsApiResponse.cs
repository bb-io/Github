using Newtonsoft.Json;

namespace Apps.GitHub.Models.Workflow;

public class WorkflowJobsApiResponse
{
    [JsonProperty("total_count")]
    public int TotalCount { get; set; }

    [JsonProperty("jobs")]
    public List<WorkflowJobApiResponse> Jobs { get; set; } = [];
}

public class WorkflowJobApiResponse
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("name")]
    public string? Name { get; set; }

    [JsonProperty("status")]
    public string? Status { get; set; }

    [JsonProperty("conclusion")]
    public string? Conclusion { get; set; }

    [JsonProperty("url")]
    public string? Url { get; set; }

    [JsonProperty("html_url")]
    public string? HtmlUrl { get; set; }

    [JsonProperty("check_run_url")]
    public string? CheckRunUrl { get; set; }

    [JsonProperty("started_at")]
    public DateTime? StartedAt { get; set; }

    [JsonProperty("completed_at")]
    public DateTime? CompletedAt { get; set; }

    [JsonProperty("steps")]
    public List<WorkflowJobStepApiResponse> Steps { get; set; } = [];
}

public class WorkflowJobStepApiResponse
{
    [JsonProperty("number")]
    public int Number { get; set; }

    [JsonProperty("name")]
    public string? Name { get; set; }

    [JsonProperty("status")]
    public string? Status { get; set; }

    [JsonProperty("conclusion")]
    public string? Conclusion { get; set; }

    [JsonProperty("started_at")]
    public DateTime? StartedAt { get; set; }

    [JsonProperty("completed_at")]
    public DateTime? CompletedAt { get; set; }
}
