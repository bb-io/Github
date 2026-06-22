using Blackbird.Applications.Sdk.Common;

namespace Apps.GitHub.Models.Workflow;

public class WorkflowRunResponseDto
{
    public string Repository { get; set; } = default!;

    [Display("Workflow run ID")]
    public long WorkflowRunId { get; set; }

    [Display("Workflow ID")]
    public long WorkflowId { get; set; }

    [Display("Workflow name")]
    public string? WorkflowName { get; set; }

    [Display("Display title")]
    public string? DisplayTitle { get; set; }

    public string? Event { get; set; }
    public string? Status { get; set; }
    public string? Conclusion { get; set; }

    [Display("Head branch")]
    public string? HeadBranch { get; set; }

    [Display("Head SHA")]
    public string? HeadSha { get; set; }

    [Display("Run attempt")]
    public int? RunAttempt { get; set; }

    [Display("Run URL")]
    public string? RunUrl { get; set; }

    [Display("HTML URL")]
    public string? HtmlUrl { get; set; }

    [Display("Jobs URL")]
    public string? JobsUrl { get; set; }

    [Display("Logs URL")]
    public string? LogsUrl { get; set; }

    [Display("Created at")]
    public DateTime? CreatedAt { get; set; }

    [Display("Updated at")]
    public DateTime? UpdatedAt { get; set; }

    [Display("Run started at")]
    public DateTime? RunStartedAt { get; set; }
}
