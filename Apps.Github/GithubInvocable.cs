using Apps.GitHub.Api;
using Apps.GitHub.Models.Workflow;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Octokit;
using RestSharp;

namespace Apps.GitHub;

public class GithubInvocable : BaseInvocable
{
    protected const string GithubApiVersion = "2026-03-10";

    public IEnumerable<AuthenticationCredentialsProvider> Creds =>
        InvocationContext.AuthenticationCredentialsProviders;

    protected GithubOctokitClient ClientSdk { get; }
    protected GithubRestClient ClientRest { get; }

    public GithubInvocable(InvocationContext invocationContext) : base(invocationContext)
    {
        ClientSdk = new(Creds);
        ClientRest = new(Creds);
    }

    protected bool IsUsingPersonalAccessToken => Creds.First(p => p.KeyName == "Authorization").Value.StartsWith("github_pat");

    protected async Task ExecuteWithErrorHandlingAsync(Func<Task> action)
    {
        try
        {
            await action();
        }
        catch (Exception ex)
        {
            throw new PluginApplicationException(ex.Message);
        }
    }

    protected async Task<T> ExecuteWithErrorHandlingAsync<T>(Func<Task<T>> action)
    {
        try
        {
            return await action();
        }
        catch (Exception ex)
        {
            throw new PluginApplicationException(ex.Message);
        }
    }

    protected async Task<Repository> GetRepositoryAsync(string repositoryId) =>
        await ExecuteWithErrorHandlingAsync(async () => await ClientSdk.Repository.Get(long.Parse(repositoryId)));

    protected async Task<WorkflowDispatchApiResponse> DispatchWorkflowAsync(Repository repository,
        TriggerWorkflowRequest input)
    {
        IDictionary<string, object>? inputsDict = null;
        if (!string.IsNullOrWhiteSpace(input.InputsJson))
        {
            try
            {
                var obj = Newtonsoft.Json.Linq.JObject.Parse(input.InputsJson);
                if (obj.Properties().Any())
                    inputsDict = obj.ToObject<Dictionary<string, object>>();
            }
            catch (Newtonsoft.Json.JsonException ex)
            {
                throw new PluginMisconfigurationException($"Invalid JSON in 'Inputs (JSON)': {ex.Message}");
            }
        }

        var request = new RestRequest(
            $"/{repository.Owner.Login}/{repository.Name}/actions/workflows/{input.Workflow}/dispatches",
            Method.Post);
        request.AddHeader("Accept", "application/vnd.github+json");
        request.AddHeader("X-GitHub-Api-Version", GithubApiVersion);

        var body = new Dictionary<string, object>
        {
            ["ref"] = input.Ref
        };

        if (inputsDict is not null)
        {
            body["inputs"] = inputsDict;
        }

        request.AddJsonBody(body);

        return await ClientRest.ExecuteWithErrorHandling<WorkflowDispatchApiResponse>(request);
    }

    protected static long ParseWorkflowRunId(string workflowRunId)
    {
        if (!long.TryParse(workflowRunId, out var parsedId))
        {
            throw new PluginMisconfigurationException("Workflow run ID must be a valid integer.");
        }

        return parsedId;
    }

    protected async Task<WorkflowRunResponseDto> GetWorkflowRunAsync(string repositoryId, long workflowRunId)
    {
        var repository = await GetRepositoryAsync(repositoryId);
        return await GetWorkflowRunAsync(repository, workflowRunId);
    }

    protected async Task<WorkflowRunResponseDto> GetWorkflowRunAsync(Repository repository, long workflowRunId)
    {
        var request = new RestRequest($"/{repository.Owner.Login}/{repository.Name}/actions/runs/{workflowRunId}",
            Method.Get);
        request.AddHeader("Accept", "application/vnd.github+json");
        request.AddHeader("X-GitHub-Api-Version", GithubApiVersion);

        var workflowRun = await ClientRest.ExecuteWithErrorHandling<WorkflowRunApiResponse>(request);
        return new WorkflowRunResponseDto
        {
            Repository = $"{repository.Owner.Login}/{repository.Name}",
            WorkflowRunId = workflowRun.Id,
            WorkflowId = workflowRun.WorkflowId,
            WorkflowName = workflowRun.Name,
            DisplayTitle = workflowRun.DisplayTitle,
            Event = workflowRun.Event,
            Status = workflowRun.Status,
            Conclusion = workflowRun.Conclusion,
            HeadBranch = workflowRun.HeadBranch,
            HeadSha = workflowRun.HeadSha,
            RunAttempt = workflowRun.RunAttempt,
            HtmlUrl = workflowRun.HtmlUrl,
            RunUrl = workflowRun.Url,
            JobsUrl = workflowRun.JobsUrl,
            LogsUrl = workflowRun.LogsUrl,
            CreatedAt = workflowRun.CreatedAt,
            UpdatedAt = workflowRun.UpdatedAt,
            RunStartedAt = workflowRun.RunStartedAt
        };
    }
}
