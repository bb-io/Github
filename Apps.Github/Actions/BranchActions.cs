using Apps.Github.Dtos;
using Apps.Github.Models.Branch.Requests;
using Apps.Github.Models.Branch.Responses;
using Apps.Github.Models.Respository.Requests;
using Apps.Github.Webhooks.Payloads;
using Apps.GitHub.Models.Branch.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;
using Octokit;

namespace Apps.Github.Actions;

[ActionList]
public class BranchActions
{
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

    [Action("Merge branch", Description = "Merge branch")]
    public async Task<MergeDto> MergeBranch(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] GetRepositoryRequest repositoryRequest,
        [ActionParameter] MergeBranchRequest input)
    {
        var client = new BlackbirdGithubClient(authenticationCredentialsProviders);
        var mergeRequest = new NewMerge(input.BaseBranch, input.HeadBranch) { CommitMessage = input.CommitMessage };
        var merge = await client.Repository.Merging.Create(long.Parse(repositoryRequest.RepositoryId), mergeRequest);
        return new(merge);
    }

    [Action("Create branch", Description = "Create branch")]
    public async Task<BranchDto> CreateBranch(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] GetRepositoryRequest repositoryRequest,
        [ActionParameter] CreateBranchRequest input)
    {
        var client = new BlackbirdGithubClient(authenticationCredentialsProviders);
        var master = await client.Git.Reference.Get(long.Parse(repositoryRequest.RepositoryId),
            $"heads/{input.BaseBranchName}");
        await client.Git.Reference.Create(long.Parse(repositoryRequest.RepositoryId),
            new("refs/heads/" + input.NewBranchName, master.Object.Sha));
        return await GetBranch(authenticationCredentialsProviders, repositoryRequest,
            new() { Name = input.NewBranchName });
    }
}