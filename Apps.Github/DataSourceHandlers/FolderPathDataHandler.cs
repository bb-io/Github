
using Apps.GitHub.Models.Branch.Requests;
using Apps.Github.Models.Respository.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using Octokit;

namespace Apps.GitHub.DataSourceHandlers;

public class FolderPathDataHandler : GithubInvocable, IAsyncDataSourceHandler
{
    public GetRepositoryRequest RepositoryRequest { get; set; }
    public GetOptionalBranchRequest BranchRequest { get; set; }

    private const int VisibleFilePathSymbolsNumber = 40;

    public FolderPathDataHandler(InvocationContext invocationContext,
        [ActionParameter] GetRepositoryRequest repositoryRequest,
        [ActionParameter] GetOptionalBranchRequest branchRequest) : base(invocationContext)
    {
        RepositoryRequest = repositoryRequest;
        BranchRequest = branchRequest;
    }

    public async Task<Dictionary<string, string>> GetDataAsync(
            DataSourceContext context,
            CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(RepositoryRequest.RepositoryId))
            throw new ArgumentException("Please, specify repository first");

        var repositoryInfo = await ClientSdk.Repository.Get(long.Parse(RepositoryRequest.RepositoryId));

        var tree = await ClientSdk.Git.Tree.GetRecursive(long.Parse(RepositoryRequest.RepositoryId), BranchRequest?.Name ?? repositoryInfo.DefaultBranch);
        var result = tree.Tree
            .Where(x => x.Type.Value == TreeType.Tree)
            .Where(x => context.SearchString == null || x.Path.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .Take(30).ToDictionary(x => x.Path, x => x.Path.Length > VisibleFilePathSymbolsNumber ? x.Path[^VisibleFilePathSymbolsNumber..] : x.Path);
        result.Add("/", "Repository root");
        return result;
    }
}
