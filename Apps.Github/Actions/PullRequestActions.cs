﻿using Apps.Github.Actions.Base;
using Apps.Github.Dtos;
using Apps.Github.Models.PullRequest.Requests;
using Apps.Github.Models.Respository.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
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

    [Action("Create pull request", Description = "Create pull request")]
    public async Task<PullRequestDto> CreatePullRequest(
        [ActionParameter] GetRepositoryRequest repositoryRequest,
        [ActionParameter] CreatePullRequest input)
    {
        var pullRequest = new NewPullRequest(input.Title, input.HeadBranch, input.BaseBranch) { Body = input.Description };
        var pull = await ClientSdk.PullRequest.Create(long.Parse(repositoryRequest.RepositoryId), pullRequest);
        return new(pull);
    }
}