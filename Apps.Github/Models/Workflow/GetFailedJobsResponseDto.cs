using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.GitHub.Models.Workflow;

public class GetFailedJobsResponseDto
{
    [Display("Workflow run ID")]
    public long WorkflowRunId { get; set; }

    [Display("Failed jobs")]
    public IEnumerable<FailedJobDto> FailedJobs { get; set; } = [];
}

public class FailedJobDto
{
    [Display("Job ID")]
    public long JobId { get; set; }

    public string? Name { get; set; }
    public string? Status { get; set; }
    public string? Conclusion { get; set; }

    [Display("Job URL")]
    public string? JobUrl { get; set; }

    [Display("HTML URL")]
    public string? HtmlUrl { get; set; }

    [Display("Started at")]
    public DateTime? StartedAt { get; set; }

    [Display("Completed at")]
    public DateTime? CompletedAt { get; set; }

    [Display("Failed steps")]
    public IEnumerable<FailedJobStepDto> FailedSteps { get; set; } = [];

    public IEnumerable<CheckRunAnnotationDto> Annotations { get; set; } = [];

    public FileReference? Logs { get; set; }

    [Display("Error summary")]
    public string? ErrorSummary { get; set; }

    [Display("Log retrieval error")]
    public string? LogRetrievalError { get; set; }
}

public class FailedJobStepDto
{
    public int Number { get; set; }
    public string? Name { get; set; }
    public string? Status { get; set; }
    public string? Conclusion { get; set; }

    [Display("Started at")]
    public DateTime? StartedAt { get; set; }

    [Display("Completed at")]
    public DateTime? CompletedAt { get; set; }
}

public class CheckRunAnnotationDto
{
    public string? Path { get; set; }

    [Display("Start line")]
    public int StartLine { get; set; }

    [Display("End line")]
    public int EndLine { get; set; }

    [Display("Start column")]
    public int? StartColumn { get; set; }

    [Display("End column")]
    public int? EndColumn { get; set; }

    [Display("Annotation level")]
    public string? AnnotationLevel { get; set; }

    public string? Title { get; set; }
    public string? Message { get; set; }

    [Display("Raw details")]
    public string? RawDetails { get; set; }

    [Display("Blob URL")]
    public string? BlobUrl { get; set; }
}
