using Apps.Github.Actions.Base;
using Apps.Github.Dtos;
using Apps.Github.Models.PullRequest.Requests;
using Apps.Github.Models.PullRequest.Responses;
using Apps.Github.Models.Respository.Requests;
using Apps.GitHub.Dtos;
using Apps.GitHub.Models.Commit.Responses;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using Octokit;

namespace Apps.Github.Actions;

[ActionList]
public class PullRequestActions : GithubActions
{
    private readonly IFileManagementClient _fileManagementClient;

    public PullRequestActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient)
        : base(invocationContext)
    {
        _fileManagementClient = fileManagementClient;
    }

    [Action("List pull requests", Description = "List pull requests")]
    public async Task<ListPullRequestsResponse> ListPullRequests(
        [ActionParameter] GetRepositoryRequest input)
    {
        var pulls = await Client.PullRequest.GetAllForRepository(long.Parse(input.RepositoryId));
        return new()
        {
            PullRequests = pulls.Select(p => new PullRequestDto(p))
        };
    }

    [Action("Get pull request", Description = "Get pull request")]
    public async Task<PullRequestDto> GetPullRequest(
        [ActionParameter] GetRepositoryRequest repositoryRequest,
        [ActionParameter] GetPullRequest input)
    {
        var pull = await Client.PullRequest.Get(long.Parse(repositoryRequest.RepositoryId),
            int.Parse(input.PullRequestNumber));
        return new(pull);
    }

    [Action("Create pull request", Description = "Create pull request")]
    public async Task<PullRequestDto> CreatePullRequest(
        [ActionParameter] GetRepositoryRequest repositoryRequest,
        [ActionParameter] CreatePullRequest input)
    {
        var pullRequest = new NewPullRequest(input.Title, input.HeadBranch, input.BaseBranch)
        {
            Body = input.Description
        };
        var pull = await Client.PullRequest.Create(long.Parse(repositoryRequest.RepositoryId), pullRequest);
        return new(pull);
    }

    [Action("Merge pull request", Description = "Merge pull request")]
    public Task<PullRequestMerge> MergePullRequest(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] GetRepositoryRequest repositoryRequest,
        [ActionParameter] Models.PullRequest.Requests.MergePullRequest input)
    {
        return Client.PullRequest.Merge(long.Parse(repositoryRequest.RepositoryId), int.Parse(input.PullRequestNumber),
            new());
    }

    [Action("List pull request files", Description = "List pull request files")]
    public async Task<PullRequestFilesResponse> ListPullRequestFiles(
        [ActionParameter] GetRepositoryRequest repositoryRequest,
        [ActionParameter] ListPullFilesRequest input)
    {
        var commits = await Client.PullRequest.Commits(long.Parse(repositoryRequest.RepositoryId), int.Parse(input.PullRequestNumber));
        var commitActions = new CommitActions(InvocationContext, _fileManagementClient);
        var commitsFiles = new List<GitHubCommitFile>();
        foreach (var commit in commits) 
        {
            var files = await commitActions.GetCommitWithPaginatedFiles(repositoryRequest.RepositoryId, commit.Sha);
            commitsFiles.AddRange(files.Item2);
        }
        return new(commitsFiles.DistinctBy(x => x.Sha).Select(x => new PullRequestFileDto(x)).ToList());
    }

    [Action("List pull request commits", Description = "List pull request commits")]
    public async Task<ListPullRequestCommitsResponse> ListPullRequestCommits(
        [ActionParameter] GetRepositoryRequest repositoryRequest,
        [ActionParameter] GetPullRequest input)
    {
        var commits = await Client.PullRequest.Commits(long.Parse(repositoryRequest.RepositoryId),
            int.Parse(input.PullRequestNumber));
        return new()
        {
            Commits = commits.Select(p => new PullRequestCommitDto(p))
        };
    }

    [Action("Is pull request merged", Description = "Is pull request merged")]
    public async Task<IsPullMergedResponse> IsPullMerged(
        [ActionParameter] GetRepositoryRequest repositoryRequest,
        [ActionParameter] GetPullRequest input)
    {
        return new()
        {
            IsPullMerged = await Client.PullRequest
                .Merged(long.Parse(repositoryRequest.RepositoryId), int.Parse(input.PullRequestNumber)),
        };
    }
}