using Newtonsoft.Json;

namespace Apps.GitHub.Models.Workflow.Payloads;

public class ListWorkflowsResponse
{
    [JsonProperty("workflows")]
    public IEnumerable<WorkflowLite> Workflows { get; set; } = [];
}