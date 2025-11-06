using Apps.Github.Models.Respository.Requests;
using Apps.GitHub.Models.Workflow;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Octokit;

namespace Apps.GitHub.Actions
{
    [ActionList("Workflows")]
    public class WorkflowActions(InvocationContext invocationContext) : GithubInvocable(invocationContext)
    {
        [Action("Trigger a workflow", Description = "Trigger a GitHub Actions workflow via workflow_dispatch")]
        public async Task<WorkflowDispatchResponseDto> TriggerWorkflow(
        [ActionParameter] GetRepositoryRequest repositoryRequest,
        [ActionParameter] TriggerWorkflowRequest input)
        {
            var repo = await ExecuteWithErrorHandlingAsync(async () => await ClientSdk.Repository.Get(long.Parse(repositoryRequest.RepositoryId)));

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
                    throw new PluginMisconfigurationException($"Invalid JSON in “Inputs (JSON)”: {ex.Message}");
                }
            }

            var dispatch = new CreateWorkflowDispatch(input.Ref);
            if (inputsDict != null)
                dispatch.Inputs = inputsDict;

            await ExecuteWithErrorHandlingAsync(async () => await ClientSdk.Actions.Workflows.CreateDispatch(repo.Owner.Login, repo.Name, input.Workflow, dispatch));

            return new WorkflowDispatchResponseDto
            {
                Repository = $"{repo.Owner.Login}/{repo.Name}",
                Workflow = input.Workflow,
                Ref = input.Ref,
                Inputs = inputsDict,
                Status = "requested"
            };
        }
    }
}
