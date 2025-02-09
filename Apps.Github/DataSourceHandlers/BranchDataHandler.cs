using Apps.Github.Models.Respository.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.GitHub.DataSourceHandlers;

public class BranchDataHandler : GithubInvocable, IAsyncDataSourceItemHandler
{
    private GetRepositoryRequest RepositoryRequest { get; set; }

    public BranchDataHandler(InvocationContext invocationContext, [ActionParameter] GetRepositoryRequest repositoryRequest) : base(invocationContext)
    {
        RepositoryRequest = repositoryRequest;
    }

    async Task<IEnumerable<DataSourceItem>> IAsyncDataSourceItemHandler.GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
    {
        if (RepositoryRequest == null || string.IsNullOrWhiteSpace(RepositoryRequest.RepositoryId))
            throw new PluginMisconfigurationException("Please, specify repository first");

        var branches = await ClientSdk.Repository.Branch.GetAll(long.Parse(RepositoryRequest.RepositoryId));

        return branches
            .Where(x => context.SearchString == null ||
                        x.Name.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .Take(20)
            .Select(x => new DataSourceItem(x.Name, x.Name)); //TODO name + name looks unintentional
    }
}
