using Apps.Github.Models.Respository.Requests;
using Apps.GitHub.Models.Workflow;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.GitHub.Actions;

[ActionList("Workflows")]
public class WorkflowActions(InvocationContext invocationContext) : GithubInvocable(invocationContext)
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
}
