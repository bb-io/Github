using Newtonsoft.Json;

namespace Apps.GitHub.Models.Workflow;

public class WorkflowDispatchApiResponse
{
    [JsonProperty("workflow_run_id")]
    public long WorkflowRunId { get; set; }

    [JsonProperty("run_url")]
    public string? RunUrl { get; set; }

    [JsonProperty("html_url")]
    public string? HtmlUrl { get; set; }
}
