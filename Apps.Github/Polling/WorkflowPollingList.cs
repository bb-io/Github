using Apps.Github.Models.Respository.Requests;
using Apps.GitHub.Models.Workflow;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Polling;

namespace Apps.GitHub.Polling;

[PollingEventList("Workflows")]
public class WorkflowPollingList(InvocationContext invocationContext) : GithubInvocable(invocationContext)
{
    [PollingEvent("On workflow run completed",
        Description = "Triggered when the specified GitHub Actions workflow run reaches completed status")]
    public async Task<PollingEventResponse<WorkflowRunPollingMemory, WorkflowRunResponseDto>> OnWorkflowRunCompleted(
        PollingEventRequest<WorkflowRunPollingMemory> request,
        [PollingEventParameter] GetRepositoryRequest repositoryRequest,
        [PollingEventParameter] GetWorkflowRunRequest input)
    {
        var workflowRunId = ParseWorkflowRunId(input.WorkflowRunId);
        var workflowRun = await GetWorkflowRunAsync(repositoryRequest.RepositoryId, workflowRunId);

        var memory = request.Memory ?? new WorkflowRunPollingMemory();
        memory.LastKnownStatus = workflowRun.Status;
        memory.LastCheckedUtc = DateTime.UtcNow;

        var completed = string.Equals(workflowRun.Status, "completed", StringComparison.OrdinalIgnoreCase);
        var shouldTrigger = completed && !memory.Triggered;

        if (shouldTrigger)
        {
            memory.Triggered = true;
        }

        return new()
        {
            FlyBird = shouldTrigger,
            Result = shouldTrigger ? workflowRun : null,
            Memory = memory
        };
    }
}
