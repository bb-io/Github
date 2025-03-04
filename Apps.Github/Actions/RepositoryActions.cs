using Blackbird.Applications.Sdk.Common;
using Apps.Github.Dtos;
using Blackbird.Applications.Sdk.Common.Actions;
using Apps.Github.Models.Respository.Responses;
using Apps.Github.Models.Respository.Requests;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using Apps.GitHub;

namespace Apps.Github.Actions;

[ActionList]
public class RepositoryActions(InvocationContext invocationContext) : GithubInvocable(invocationContext)
{
    [Action("Get repository issues", Description = "Get opened issues against repository")]
    public async Task<GetIssuesResponse> GetIssuesInRepository([ActionParameter] RepositoryRequest input)
    {
        var issues = await ExecuteWithErrorHandlingAsync(async () => await ClientSdk.Issue.GetAllForRepository(long.Parse(input.RepositoryId)));

        return new()
        {
            Issues = issues.Select(issue => new IssueDto(issue))
        };
    }

    [Action("Debug", Description = "Debug")]
    public string Debug()
    {
        return InvocationContext.AuthenticationCredentialsProviders.First(p => p.KeyName == "Authorization").Value;
    }
}