using Apps.GitHub.Api;
using Apps.GitHub.Models.Workflow;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using RestSharp;
using System.Net.Mime;

namespace Apps.GitHub.Services;

internal class WorkflowJobsService(
    GithubRestClient client,
    IFileManagementClient fileManagementClient,
    InvocationContext invocationContext)
{
    public async Task<List<WorkflowJobApiResponse>> GetWorkflowJobsAsync(
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

            var response = await client.ExecuteWithErrorHandling<WorkflowJobsApiResponse>(request);
            jobs.AddRange(response.Jobs);

            if (response.Jobs.Count < pageSize || jobs.Count >= response.TotalCount)
                return jobs;
        }
    }

    public async Task<List<CheckRunAnnotationApiResponse>> GetCheckRunAnnotationsAsync(
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

            var response = await client.ExecuteWithErrorHandling<List<CheckRunAnnotationApiResponse>>(request);
            annotations.AddRange(response);

            if (response.Count < pageSize)
                return annotations;
        }
    }

    public async Task<JobLogResult> DownloadJobLogAsync(
        string owner, string repository, WorkflowJobApiResponse job)
    {
        try
        {
            var request = new RestRequest($"/{owner}/{repository}/actions/jobs/{job.Id}/logs", Method.Get);
            var response = await client.ExecuteAsync(request);
            if (!response.IsSuccessful)
            {
                return new JobLogResult(null,
                    $"GitHub returned HTTP {(int)response.StatusCode} while retrieving the job log.");
            }

            var logBytes = response.RawBytes;
            if (logBytes is null)
            {
                return new JobLogResult(null,
                    "GitHub returned an empty response while retrieving the job log.");
            }

            await using var logStream = new MemoryStream(logBytes, writable: false);
            var logs = await fileManagementClient.UploadAsync(logStream, MediaTypeNames.Text.Plain,
                $"workflow-job-{job.Id}.log.txt");

            return new JobLogResult(logs, null);
        }
        catch (Exception exception)
        {
            invocationContext.Logger?.LogWarning(
                $"Could not retrieve logs for workflow job {job.Id}: {exception.Message}", []);
            return new JobLogResult(null, "The job log could not be retrieved from GitHub.");
        }
    }
}

internal record JobLogResult(FileReference? Logs, string? RetrievalError);
