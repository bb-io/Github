using Blackbird.Applications.Sdk.Common;

namespace Apps.GitHub.Models.Workflow;

public class GetWorkflowRunRequest
{
    [Display("Workflow run ID")]
    public string WorkflowRunId { get; set; } = default!;
}
