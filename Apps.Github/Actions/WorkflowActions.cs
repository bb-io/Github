using Apps.Github.Models.Respository.Requests;
using Apps.GitHub.Models.Workflow;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using RestSharp;
using System.Net.Mime;
using System.Text;

namespace Apps.GitHub.Actions;

[ActionList("Workflows")]
public class WorkflowActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient)
    : GithubInvocable(invocationContext)
{
    [Action("Trigger a workflow", Description = "Trigger a GitHub Actions workflow via workflow_dispatch")]
    public async Task<WorkflowDispatchResponseDto> TriggerWorkflow(
        [ActionParameter] GetRepositoryRequest repositoryRequest,
        [ActionParameter] TriggerWorkflowRequest input)
    {
        var repo = await GetRepositoryAsync(repositoryRequest.RepositoryId);
        var dispatch = await DispatchWorkflowAsync(repo, input);

        return new WorkflowDispatchResponseDto
        {
            Repository = $"{repo.Owner.Login}/{repo.Name}",
            Workflow = input.Workflow,
            Ref = input.Ref,
            InputsJson = input.InputsJson,
            Status = "requested",
            WorkflowRunId = dispatch.WorkflowRunId,
            RunUrl = dispatch.RunUrl,
            HtmlUrl = dispatch.HtmlUrl
        };
    }

    [Action("Get workflow run", Description = "Get the current status and result of a GitHub Actions workflow run")]
    public async Task<WorkflowRunResponseDto> GetWorkflowRun(
        [ActionParameter] GetRepositoryRequest repositoryRequest,
        [ActionParameter] GetWorkflowRunRequest input)
    {
        var workflowRunId = ParseWorkflowRunId(input.WorkflowRunId);
        return await GetWorkflowRunAsync(repositoryRequest.RepositoryId, workflowRunId);
    }

    [Action("Get failed jobs", Description = "Get failed, timed out, and cancelled jobs for the latest workflow run attempt")]
    public async Task<GetFailedJobsResponseDto> GetFailedJobs(
        [ActionParameter] GetRepositoryRequest repositoryRequest,
        [ActionParameter] GetWorkflowRunRequest input)
    {
        var workflowRunId = ParseWorkflowRunId(input.WorkflowRunId);
        var repository = await GetRepositoryAsync(repositoryRequest.RepositoryId);
        var jobs = await GetWorkflowJobsAsync(repository.Owner.Login, repository.Name, workflowRunId);
        var failedJobs = new List<FailedJobDto>();

        foreach (var job in jobs.Where(job => IsFailedConclusion(job.Conclusion)))
        {
            var checkRunId = GetCheckRunId(job);
            var annotations = await GetCheckRunAnnotationsAsync(repository.Owner.Login, repository.Name, checkRunId);
            var logResult = await DownloadJobLogAsync(repository.Owner.Login, repository.Name, job, annotations);

            failedJobs.Add(new FailedJobDto
            {
                JobId = job.Id,
                Name = job.Name,
                Status = job.Status,
                Conclusion = job.Conclusion,
                JobUrl = job.Url,
                HtmlUrl = job.HtmlUrl,
                StartedAt = job.StartedAt,
                CompletedAt = job.CompletedAt,
                FailedSteps = job.Steps
                    .Where(step => IsFailedConclusion(step.Conclusion))
                    .Select(MapFailedStep)
                    .ToList(),
                Annotations = annotations.Select(MapAnnotation).ToList(),
                Logs = logResult.Logs,
                ErrorSummary = logResult.ErrorSummary,
                LogRetrievalError = logResult.RetrievalError
            });
        }

        return new GetFailedJobsResponseDto
        {
            WorkflowRunId = workflowRunId,
            FailedJobs = failedJobs
        };
    }

    private async Task<List<WorkflowJobApiResponse>> GetWorkflowJobsAsync(
        string owner, string repository, long workflowRunId)
    {
        const int pageSize = 100;
        var jobs = new List<WorkflowJobApiResponse>();

        for (var page = 1; ; page++)
        {
            var request = new RestRequest(
                    $"/{owner}/{repository}/actions/runs/{workflowRunId}/jobs", Method.Get)
                .AddQueryParameter("filter", "latest")
                .AddQueryParameter("per_page", pageSize)
                .AddQueryParameter("page", page);

            var response = await ClientRest.ExecuteWithErrorHandling<WorkflowJobsApiResponse>(request);
            jobs.AddRange(response.Jobs);

            if (response.Jobs.Count < pageSize || jobs.Count >= response.TotalCount)
                return jobs;
        }
    }

    private async Task<List<CheckRunAnnotationApiResponse>> GetCheckRunAnnotationsAsync(
        string owner, string repository, long checkRunId)
    {
        const int pageSize = 100;
        var annotations = new List<CheckRunAnnotationApiResponse>();

        for (var page = 1; ; page++)
        {
            var request = new RestRequest(
                    $"/{owner}/{repository}/check-runs/{checkRunId}/annotations", Method.Get)
                .AddQueryParameter("per_page", pageSize)
                .AddQueryParameter("page", page);

            var response = await ClientRest.ExecuteWithErrorHandling<List<CheckRunAnnotationApiResponse>>(request);
            annotations.AddRange(response);

            if (response.Count < pageSize)
                return annotations;
        }
    }

    private async Task<JobLogResult> DownloadJobLogAsync(
        string owner,
        string repository,
        WorkflowJobApiResponse job,
        IEnumerable<CheckRunAnnotationApiResponse> annotations)
    {
        try
        {
            var request = new RestRequest($"/{owner}/{repository}/actions/jobs/{job.Id}/logs", Method.Get);
            var response = await ClientRest.ExecuteAsync(request);
            if (!response.IsSuccessful)
            {
                return new JobLogResult(null, CreateAnnotationSummary(annotations),
                    $"GitHub returned HTTP {(int)response.StatusCode} while retrieving the job log.");
            }

            var logBytes = response.RawBytes;
            if (logBytes is null)
            {
                return new JobLogResult(null, CreateAnnotationSummary(annotations),
                    "GitHub returned an empty response while retrieving the job log.");
            }

            var logText = Encoding.UTF8.GetString(logBytes);
            await using var logStream = new MemoryStream(logBytes, writable: false);
            var logs = await fileManagementClient.UploadAsync(logStream, MediaTypeNames.Text.Plain,
                $"workflow-job-{job.Id}.log.txt");

            return new JobLogResult(logs, CreateErrorSummary(logText, annotations), null);
        }
        catch (Exception exception)
        {
            InvocationContext.Logger?.LogWarning(
                $"Could not retrieve logs for workflow job {job.Id}: {exception.Message}", []);
            return new JobLogResult(null, CreateAnnotationSummary(annotations),
                "The job log could not be retrieved from GitHub.");
        }
    }

    internal static bool IsFailedConclusion(string? conclusion) =>
        string.Equals(conclusion, "failure", StringComparison.OrdinalIgnoreCase) ||
        string.Equals(conclusion, "timed_out", StringComparison.OrdinalIgnoreCase) ||
        string.Equals(conclusion, "cancelled", StringComparison.OrdinalIgnoreCase);

    private static long GetCheckRunId(WorkflowJobApiResponse job)
    {
        if (Uri.TryCreate(job.CheckRunUrl, UriKind.Absolute, out var checkRunUri) &&
            long.TryParse(checkRunUri.Segments.Last().Trim('/'), out var checkRunId))
        {
            return checkRunId;
        }

        if (job.Id > 0)
            return job.Id;

        throw new PluginApplicationException($"Could not determine the check run ID for job '{job.Name}'.");
    }

    private static FailedJobStepDto MapFailedStep(WorkflowJobStepApiResponse step) => new()
    {
        Number = step.Number,
        Name = step.Name,
        Status = step.Status,
        Conclusion = step.Conclusion,
        StartedAt = step.StartedAt,
        CompletedAt = step.CompletedAt
    };

    private static CheckRunAnnotationDto MapAnnotation(CheckRunAnnotationApiResponse annotation) => new()
    {
        Path = annotation.Path,
        StartLine = annotation.StartLine,
        EndLine = annotation.EndLine,
        StartColumn = annotation.StartColumn,
        EndColumn = annotation.EndColumn,
        AnnotationLevel = annotation.AnnotationLevel,
        Title = annotation.Title,
        Message = annotation.Message,
        RawDetails = annotation.RawDetails,
        BlobUrl = annotation.BlobHref
    };

    internal static string? CreateErrorSummary(
        string log,
        IEnumerable<CheckRunAnnotationApiResponse> annotations)
    {
        const int maxSummaryLines = 20;
        var errorLines = log.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries)
            .Where(line => line.Contains("##[error]", StringComparison.OrdinalIgnoreCase))
            .Select(line => line[(line.IndexOf("##[error]", StringComparison.OrdinalIgnoreCase) + "##[error]".Length)..].Trim())
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .Distinct(StringComparer.Ordinal)
            .Take(maxSummaryLines)
            .ToList();

        var summary = errorLines.Count > 0
            ? string.Join(Environment.NewLine, errorLines)
            : CreateAnnotationSummary(annotations);

        return TruncateSummary(summary);
    }

    private static string? CreateAnnotationSummary(IEnumerable<CheckRunAnnotationApiResponse> annotations)
    {
        const int maxSummaryLines = 20;
        var messages = annotations
            .Where(annotation => string.Equals(annotation.AnnotationLevel, "failure", StringComparison.OrdinalIgnoreCase))
            .Select(annotation => annotation.Message)
            .Where(message => !string.IsNullOrWhiteSpace(message))
            .Distinct(StringComparer.Ordinal)
            .Take(maxSummaryLines);

        var summary = string.Join(Environment.NewLine, messages);
        return TruncateSummary(summary.Length > 0 ? summary : null);
    }

    private static string? TruncateSummary(string? summary)
    {
        const int maxSummaryLength = 4000;
        return summary?.Length > maxSummaryLength ? summary[..maxSummaryLength] + "..." : summary;
    }

    private sealed record JobLogResult(FileReference? Logs, string? ErrorSummary, string? RetrievalError);
}
