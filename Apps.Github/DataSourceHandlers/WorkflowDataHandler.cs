using Apps.Github.Models.Respository.Requests;
using Apps.GitHub.Models.Workflow.Payloads;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.GitHub.DataSourceHandlers;

public class WorkflowDataHandler : GithubInvocable, IAsyncDataSourceItemHandler
{
    private readonly long _repositoryId;
    
    public WorkflowDataHandler(InvocationContext invocationContext, [ActionParameter] GetRepositoryRequest repositoryRequest) 
        : base(invocationContext)
    {
        if (string.IsNullOrWhiteSpace(repositoryRequest.RepositoryId))
            throw new PluginMisconfigurationException("Please specify a repository ID first");

        if (!long.TryParse(repositoryRequest.RepositoryId, out long repoId))
            throw new PluginMisconfigurationException("Incorrect repository ID. Please specify a numeric value");
            
        _repositoryId = repoId;
    }

    public async Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context,CancellationToken cancellationToken)
    {
        var repo = await ExecuteWithErrorHandlingAsync(async () => await ClientSdk.Repository.Get(_repositoryId));

        var request = new RestRequest($"/{repo.Owner.Login}/{repo.Name}/actions/workflows").AddQueryParameter("per_page", 100);
        var response = await ClientRest.ExecuteWithErrorHandling<ListWorkflowsResponse>(request);

        var workflows = response.Workflows;
        
        var search = context.SearchString?.Trim();
        if (!string.IsNullOrEmpty(search))
        {
            workflows = workflows.Where(w =>
                w.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                w.Path.Contains(search, StringComparison.OrdinalIgnoreCase));
        }

        return workflows
            .OrderBy(w => w.Name, StringComparer.OrdinalIgnoreCase)
            .Select(w =>
            {
                string fileName = Path.GetFileName(w.Path);
                string label = string.IsNullOrEmpty(fileName) ? w.Name : $"{w.Name} ({fileName})";
                return new DataSourceItem(w.Id.ToString(), label);
            })
            .ToList();
    }
}
