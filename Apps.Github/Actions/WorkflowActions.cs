using Apps.Github.Models.Respository.Requests;
using Apps.GitHub.Models.Workflow;
using Apps.GitHub.Services;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;

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
        var workflowJobsService = new WorkflowJobsService(ClientRest, fileManagementClient, InvocationContext);
        var jobs = await workflowJobsService.GetWorkflowJobsAsync(
            repository.Owner.Login, repository.Name, workflowRunId);
        var failedJobs = new List<FailedJobDto>();

        foreach (var job in jobs.Where(job => IsFailedConclusion(job.Conclusion)))
        {
            var checkRunId = GetCheckRunId(job);
            var annotations = await workflowJobsService.GetCheckRunAnnotationsAsync(
                repository.Owner.Login, repository.Name, checkRunId);
            var logResult = await workflowJobsService.DownloadJobLogAsync(
                repository.Owner.Login, repository.Name, job);

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
                LogRetrievalError = logResult.RetrievalError
            });
        }

        return new GetFailedJobsResponseDto
        {
            WorkflowRunId = workflowRunId,
            FailedJobs = failedJobs
        };
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

}
