using Apps.Github.Dtos;
using Apps.Github.Models.Branch.Requests;
using Apps.Github.Models.Respository.Requests;
using Apps.GitHub;
using Apps.GitHub.Models.Branch.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using Octokit;

namespace Apps.Github.Actions;

[ActionList]
public class BranchActions : GithubInvocable
{
    private readonly IFileManagementClient _fileManagementClient;

    public BranchActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient)
        : base(invocationContext)
    {
        _fileManagementClient = fileManagementClient;
    }

    [Action("Branch exists", Description = "Branch exists in specified repository")]
    public async Task<bool> BranchExists(
        [ActionParameter] GetRepositoryRequest repositoryRequest,
        [ActionParameter] [Display("Branch name")]
        string branchNameRequest)
    {
        var branches = await ExecuteWithErrorHandlingAsync(async () => await ClientSdk.Repository.Branch.GetAll(long.Parse(repositoryRequest.RepositoryId)));
        return branches.Any(x => x.Name == branchNameRequest);
    }

    [Action("Merge branch", Description = "Merge branch")]
    public async Task<MergeDto> MergeBranch(
        [ActionParameter] GetRepositoryRequest repositoryRequest,
        [ActionParameter] MergeBranchRequest input)
    {
        var mergeRequest = new NewMerge(input.BaseBranch, input.HeadBranch) { CommitMessage = input.CommitMessage };
        var merge = await ExecuteWithErrorHandlingAsync(async () => await ClientSdk.Repository.Merging.Create(long.Parse(repositoryRequest.RepositoryId), mergeRequest));
        return new(merge);
    }

    [Action("Create branch", Description = "Create branch")]
    public async Task<BranchDto> CreateBranch(
        [ActionParameter] GetRepositoryRequest repositoryRequest,
        [ActionParameter] CreateBranchRequest input)
    {
        var master = await ExecuteWithErrorHandlingAsync(async () => await ClientSdk.Git.Reference.Get(long.Parse(repositoryRequest.RepositoryId),
            $"heads/{input.BaseBranchName}"));
        await ExecuteWithErrorHandlingAsync(async () => await ClientSdk.Git.Reference.Create(long.Parse(repositoryRequest.RepositoryId),
            new("refs/heads/" + input.NewBranchName, master.Object.Sha)));
        return new(await ExecuteWithErrorHandlingAsync(async () => await ClientSdk.Repository.Branch.Get(long.Parse(repositoryRequest.RepositoryId), input.NewBranchName)));
    }
}