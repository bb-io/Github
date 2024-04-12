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
    [Action("List branches", Description = "List respository branches")]
    public ListRepositoryBranchesResponse ListRepositoryBranches(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] GetRepositoryRequest input)
    {
        var client = new BlackbirdGithubClient(authenticationCredentialsProviders);
        var branches = client.Repository.Branch.GetAll(long.Parse(input.RepositoryId)).Result;
        return new ListRepositoryBranchesResponse
        {
            Branches = branches.Select(b => new BranchDto(b))
        };
    }

    [Action("Get branch", Description = "Get branch by name")]
    public BranchDto GetBranch(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] GetRepositoryRequest repositoryRequest,
        [ActionParameter] GetBranchRequest input)
    {
        var client = new BlackbirdGithubClient(authenticationCredentialsProviders);
        var branch = client.Repository.Branch.Get(long.Parse(repositoryRequest.RepositoryId), input.Name).Result;
        return new BranchDto(branch);
    }

    [Action("Merge branch", Description = "Merge branch")]
    public MergeDto MergeBranch(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] GetRepositoryRequest repositoryRequest,
        [ActionParameter] MergeBranchRequest input)
    {
        var client = new BlackbirdGithubClient(authenticationCredentialsProviders);
        var mergeRequest = new NewMerge(input.BaseBranch, input.HeadBranch) { CommitMessage = input.CommitMessage };
        var merge = client.Repository.Merging.Create(long.Parse(repositoryRequest.RepositoryId), mergeRequest).Result;
        return new MergeDto(merge);
    }

    [Action("Create branch", Description = "Create branch")]
    public async Task<BranchDto> CreateBranch(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] GetRepositoryRequest repositoryRequest,
        [ActionParameter] CreateBranchRequest input)
    {
        var client = new BlackbirdGithubClient(authenticationCredentialsProviders);
        var master = await client.Git.Reference.Get(long.Parse(repositoryRequest.RepositoryId), $"heads/{input.BaseBranchName}");
        await client.Git.Reference.Create(long.Parse(repositoryRequest.RepositoryId), new NewReference("refs/heads/" + input.NewBranchName, master.Object.Sha));
        return GetBranch(authenticationCredentialsProviders, repositoryRequest, new GetBranchRequest() { Name = input.NewBranchName });
    }
}