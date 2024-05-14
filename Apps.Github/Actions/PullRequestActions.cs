using Apps.Github.Dtos;
using Apps.Github.Models.PullRequest.Requests;
using Apps.Github.Models.PullRequest.Responses;
using Apps.Github.Models.Respository.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;
using Octokit;

namespace Apps.Github.Actions;

[ActionList]
public class PullRequestActions
{
    [Action("List pull requests", Description = "List pull requests")]
    public async Task<ListPullRequestsResponse> ListPullRequests(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] GetRepositoryRequest input)
    {
        var client = new BlackbirdGithubClient(authenticationCredentialsProviders);
        var pulls = await client.PullRequest.GetAllForRepository(long.Parse(input.RepositoryId));
        return new()
        {
            PullRequests = pulls.Select(p => new PullRequestDto(p))
        };
    }

    [Action("Get pull request", Description = "Get pull request")]
    public async Task<PullRequestDto> GetPullRequest(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] GetRepositoryRequest repositoryRequest,
        [ActionParameter] GetPullRequest input)
    {
        var client = new BlackbirdGithubClient(authenticationCredentialsProviders);
        var pull = await client.PullRequest.Get(long.Parse(repositoryRequest.RepositoryId),
            int.Parse(input.PullRequestNumber));
        return new(pull);
    }

    [Action("Create pull request", Description = "Create pull request")]
    public async Task<PullRequestDto> CreatePullRequest(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] GetRepositoryRequest repositoryRequest,
        [ActionParameter] CreatePullRequest input)
    {
        var client = new BlackbirdGithubClient(authenticationCredentialsProviders);
        var pullRequest = new NewPullRequest(input.Title, input.HeadBranch, input.BaseBranch)
        {
            Body = input.Description
        };
        var pull = await client.PullRequest.Create(long.Parse(repositoryRequest.RepositoryId), pullRequest);
        return new(pull);
    }

    [Action("Merge pull request", Description = "Merge pull request")]
    public Task<PullRequestMerge> MergePullRequest(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] GetRepositoryRequest repositoryRequest,
        [ActionParameter] Models.PullRequest.Requests.MergePullRequest input)
    {
        var client = new BlackbirdGithubClient(authenticationCredentialsProviders);
        return client.PullRequest.Merge(long.Parse(repositoryRequest.RepositoryId), int.Parse(input.PullRequestNumber),
            new());
    }

    [Action("List pull request files", Description = "List pull request files")]
    public async Task<ListPullRequestFilesResponse> ListPullRequestFiles(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] GetRepositoryRequest repositoryRequest,
        [ActionParameter] ListPullFilesRequest input)
    {
        var client = new BlackbirdGithubClient(authenticationCredentialsProviders);
        var files = await client.PullRequest.Files(long.Parse(repositoryRequest.RepositoryId),
            int.Parse(input.PullRequestNumber));
        return new()
        {
            Files = files
        };
    }

    [Action("List pull request commits", Description = "List pull request commits")]
    public async Task<ListPullRequestCommitsResponse> ListPullRequestCommits(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] GetRepositoryRequest repositoryRequest,
        [ActionParameter] GetPullRequest input)
    {
        var client = new BlackbirdGithubClient(authenticationCredentialsProviders);
        var commits = await client.PullRequest.Commits(long.Parse(repositoryRequest.RepositoryId),
            int.Parse(input.PullRequestNumber));
        return new()
        {
            Commits = commits.Select(p => new PullRequestCommitDto(p))
        };
    }

    [Action("Is pull request merged", Description = "Is pull request merged")]
    public async Task<IsPullMergedResponse> IsPullMerged(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] GetRepositoryRequest repositoryRequest,
        [ActionParameter] GetPullRequest input)
    {
        var client = new BlackbirdGithubClient(authenticationCredentialsProviders);
        return new()
        {
            IsPullMerged = await client.PullRequest
                .Merged(long.Parse(repositoryRequest.RepositoryId), int.Parse(input.PullRequestNumber)),
        };
    }
}