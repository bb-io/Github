using Apps.Github.Actions.Base;
using Apps.Github.Dtos;
using Apps.Github.Models.Branch.Requests;
using Apps.Github.Models.Respository.Requests;
using Apps.GitHub.Models.Branch.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using Octokit;

namespace Apps.Github.Actions;

[ActionList]
public class BranchActions : GithubActions
{
    private readonly IFileManagementClient _fileManagementClient;

    public BranchActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient)
        : base(invocationContext)
    {
        _fileManagementClient = fileManagementClient;
    }

    //[Action("List branches", Description = "List respository branches")]
    //public async Task<ListRepositoryBranchesResponse> ListRepositoryBranches(
    //    IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
    //    [ActionParameter] GetRepositoryRequest input)
    //{
    //    var client = new BlackbirdGithubClient(authenticationCredentialsProviders);
    //    var branches = await client.Repository.Branch.GetAll(long.Parse(input.RepositoryId));
    //    return new()
    //    {
    //        Branches = branches.Select(b => new BranchDto(b))
    //    };
    //}

    //[Action("Get branch", Description = "Get branch by name")]
    //public async Task<BranchDto> GetBranch(
    //    IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
    //    [ActionParameter] GetRepositoryRequest repositoryRequest,
    //    [ActionParameter] GetBranchRequest input)
    //{
    //    var client = new BlackbirdGithubClient(authenticationCredentialsProviders);
    //    var branch = await client.Repository.Branch.Get(long.Parse(repositoryRequest.RepositoryId), input.Name);
    //    return new(branch);
    //}
    [Action("Branch exists", Description = "Branch exists in specified repository")]
    public async Task<bool> BranchExists(
        [ActionParameter] GetRepositoryRequest repositoryRequest,
        [ActionParameter] [Display("Branch name")]
        string branchNameRequest)
    {
        var branches = await Client.Repository.Branch.GetAll(long.Parse(repositoryRequest.RepositoryId));
        return branches.Any(x => x.Name == branchNameRequest);
    }

    [Action("Merge branch", Description = "Merge branch")]
    public async Task<MergeDto> MergeBranch(
        [ActionParameter] GetRepositoryRequest repositoryRequest,
        [ActionParameter] MergeBranchRequest input)
    {
        var client = new BlackbirdGithubClient(Creds);
        var mergeRequest = new NewMerge(input.BaseBranch, input.HeadBranch) { CommitMessage = input.CommitMessage };
        var merge = await client.Repository.Merging.Create(long.Parse(repositoryRequest.RepositoryId), mergeRequest);
        return new(merge);
    }

    [Action("Create branch", Description = "Create branch")]
    public async Task<BranchDto> CreateBranch(
        [ActionParameter] GetRepositoryRequest repositoryRequest,
        [ActionParameter] CreateBranchRequest input)
    {
        var client = new BlackbirdGithubClient(Creds);
        var master = await client.Git.Reference.Get(long.Parse(repositoryRequest.RepositoryId),
            $"heads/{input.BaseBranchName}");
        await client.Git.Reference.Create(long.Parse(repositoryRequest.RepositoryId),
            new("refs/heads/" + input.NewBranchName, master.Object.Sha));
        return new(await client.Repository.Branch.Get(long.Parse(repositoryRequest.RepositoryId), input.NewBranchName));
    }
}