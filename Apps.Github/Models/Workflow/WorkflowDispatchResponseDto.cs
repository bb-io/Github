using Blackbird.Applications.Sdk.Common;

namespace Apps.GitHub.Models.Workflow;

public class WorkflowDispatchResponseDto
{
    public string Repository { get; set; } = default!;
    public string Workflow { get; set; } = default!;
    public string Ref { get; set; } = default!;
    public string? InputsJson { get; set; }
    public string Status { get; set; } = "requested";

    [Display("Workflow run ID")]
    public long WorkflowRunId { get; set; }

    [Display("Run URL")]
    public string? RunUrl { get; set; }

    [Display("HTML URL")]
    public string? HtmlUrl { get; set; }
}
