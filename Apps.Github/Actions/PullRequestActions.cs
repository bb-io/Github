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
    public ListPullRequestsResponse ListPullRequests(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] GetRepositoryRequest input)
    {
        var client = new BlackbirdGithubClient(authenticationCredentialsProviders);
        var pulls = client.PullRequest.GetAllForRepository(long.Parse(input.RepositoryId)).Result;
        return new ListPullRequestsResponse
        {
            PullRequests = pulls.Select(p => new PullRequestDto(p))
        };
    }

    [Action("Get pull request", Description = "Get pull request")]
    public PullRequestDto GetPullRequest(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] GetRepositoryRequest repositoryRequest,
        [ActionParameter] GetPullRequest input)
    {
        var client = new BlackbirdGithubClient(authenticationCredentialsProviders);
        var pull = client.PullRequest.Get(long.Parse(repositoryRequest.RepositoryId), int.Parse(input.PullRequestNumber)).Result;
        return new PullRequestDto(pull);
    }

    [Action("Create pull request", Description = "Create pull request")]
    public PullRequestDto CreatePullRequest(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] GetRepositoryRequest repositoryRequest,
        [ActionParameter] CreatePullRequest input)
    {
        var client = new BlackbirdGithubClient(authenticationCredentialsProviders);
        var pullRequest = new NewPullRequest(input.Title, input.HeadBranch, input.BaseBranch);
        pullRequest.Body = input.Description;
        var pull = client.PullRequest.Create(long.Parse(repositoryRequest.RepositoryId), pullRequest).Result;
        return new PullRequestDto(pull);
    }

    [Action("Merge pull request", Description = "Merge pull request")]
    public PullRequestMerge MergePullRequest(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] GetRepositoryRequest repositoryRequest,
        [ActionParameter] Models.PullRequest.Requests.MergePullRequest input)
    {
        var client = new BlackbirdGithubClient(authenticationCredentialsProviders);
        return client.PullRequest.Merge(long.Parse(repositoryRequest.RepositoryId), int.Parse(input.PullRequestNumber), new Octokit.MergePullRequest()).Result;
    }

    [Action("List pull request files", Description = "List pull request files")]
    public ListPullRequestFilesResponse ListPullRequestFiles(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] GetRepositoryRequest repositoryRequest,
        [ActionParameter] ListPullFilesRequest input)
    {
        var client = new BlackbirdGithubClient(authenticationCredentialsProviders);
        var files = client.PullRequest.Files(long.Parse(repositoryRequest.RepositoryId), int.Parse(input.PullRequestNumber)).Result;
        return new ListPullRequestFilesResponse
        {
            Files = files
        };
    }

    [Action("List pull request commits", Description = "List pull request commits")]
    public ListPullRequestCommitsResponse ListPullRequestCommits(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] GetRepositoryRequest repositoryRequest,
        [ActionParameter] GetPullRequest input)
    {
        var client = new BlackbirdGithubClient(authenticationCredentialsProviders);
        var commits = client.PullRequest.Commits(long.Parse(repositoryRequest.RepositoryId), int.Parse(input.PullRequestNumber)).Result;
        return new ListPullRequestCommitsResponse
        {
            Commits = commits.Select(p => new PullRequestCommitDto(p))
        };
    }

    [Action("Is pull request merged", Description = "Is pull request merged")]
    public IsPullMergedResponse IsPullMerged(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] GetRepositoryRequest repositoryRequest,
        [ActionParameter] GetPullRequest input)
    {
        var client = new BlackbirdGithubClient(authenticationCredentialsProviders);
        return new IsPullMergedResponse
        {
            IsPullMerged = client.PullRequest.Merged(long.Parse(repositoryRequest.RepositoryId), int.Parse(input.PullRequestNumber)).Result,
        };
    }
}