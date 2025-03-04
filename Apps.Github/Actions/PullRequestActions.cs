using Apps.Github.Dtos;
using Apps.Github.Models.PullRequest.Requests;
using Apps.Github.Models.Respository.Requests;
using Apps.GitHub;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using Octokit;

namespace Apps.Github.Actions;

[ActionList]
public class PullRequestActions(InvocationContext invocationContext)
    : GithubInvocable(invocationContext)
{
    [Action("Create pull request", Description = "Create pull request")]
    public async Task<PullRequestDto> CreatePullRequest(
        [ActionParameter] GetRepositoryRequest repositoryRequest,
        [ActionParameter] CreatePullRequest input)
    {
        var pullRequest = new NewPullRequest(input.Title, input.HeadBranch, input.BaseBranch) { Body = input.Description };
        var pull = await ExecuteWithErrorHandlingAsync(async () => await ClientSdk.PullRequest.Create(long.Parse(repositoryRequest.RepositoryId), pullRequest));
        return new(pull);
    }
}