using Apps.Github.Dtos;
using Apps.Github.Models.PullRequest.Payloads;
using Apps.Github.Models.PullRequest.Requests;
using Apps.Github.Models.PullRequest.Responses;
using Apps.Github.Models.Respository.Requests;
using Apps.GitHub;
using Apps.GitHub.Dtos;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Newtonsoft.Json;
using Octokit;
using RestSharp;
using IssueCommentPayload = Apps.Github.Models.PullRequest.Payloads.IssueCommentPayload;

namespace Apps.Github.Actions;

[ActionList("Pull requests")]
public class PullRequestActions(InvocationContext invocationContext)
    : GithubInvocable(invocationContext)
{
    [Action("Create pull request", Description = "Create pull request")]
    public async Task<PullRequestDto> CreatePullRequest(
        [ActionParameter] GetRepositoryRequest repositoryRequest,
        [ActionParameter] CreatePullRequest input)
    {
        var pullRequest = new NewPullRequest(input.Title, input.HeadBranch, input.BaseBranch) { Body = input.Description };
        var pull = await ExecuteWithErrorHandlingAsync(async () =>
            await ClientSdk.PullRequest.Create(long.Parse(repositoryRequest.RepositoryId), pullRequest));
        return new(pull);
    }

    [Action("Get pull request", Description = "Get pull request by number")]
    public async Task<PullRequestDto> GetPullRequest(
        [ActionParameter] GetRepositoryRequest repositoryRequest,
        [ActionParameter] PullRequestIdentifierRequest input)
    {
        var pullRequest = await ExecuteWithErrorHandlingAsync(async () =>
            await ClientSdk.PullRequest.Get(long.Parse(repositoryRequest.RepositoryId), ParsePullRequestNumber(input.PullRequestNumber)));

        return new PullRequestDto(pullRequest);
    }

    [Action("List pull request files", Description = "List files changed in a pull request")]
    public async Task<ListPullRequestFilesResponse> ListPullRequestFiles(
        [ActionParameter] GetRepositoryRequest repositoryRequest,
        [ActionParameter] PullRequestIdentifierRequest input)
    {
        var files = await ExecuteWithErrorHandlingAsync(async () =>
            await ClientSdk.PullRequest.Files(long.Parse(repositoryRequest.RepositoryId), ParsePullRequestNumber(input.PullRequestNumber)));

        return new()
        {
            Files = files.Select(x => new PullRequestFileDto(x)).ToList()
        };
    }

    [Action("List pull request commits", Description = "List commits included in a pull request")]
    public async Task<ListPullRequestCommitsResponse> ListPullRequestCommits(
        [ActionParameter] GetRepositoryRequest repositoryRequest,
        [ActionParameter] PullRequestIdentifierRequest input)
    {
        var commits = await ExecuteWithErrorHandlingAsync(async () =>
            await ClientSdk.PullRequest.Commits(long.Parse(repositoryRequest.RepositoryId), ParsePullRequestNumber(input.PullRequestNumber)));

        return new()
        {
            Commits = commits.Select(x => new PullRequestCommitDto(x)).ToList()
        };
    }

    [Action("List issue comments for pull request", Description = "List issue comments for a pull request")]
    public async Task<ListIssueCommentsResponse> ListIssueComments(
        [ActionParameter] GetRepositoryRequest repositoryRequest,
        [ActionParameter] PullRequestIdentifierRequest input)
    {
        var repository = await GetRepositoryAsync(repositoryRequest.RepositoryId);
        var request = CreateRestRequest($"/{repository.Owner.Login}/{repository.Name}/issues/{input.PullRequestNumber}/comments", Method.Get);
        var comments = await ClientRest.ExecuteWithErrorHandling<List<IssueCommentPayload>>(request);

        return new()
        {
            Comments = comments.Select(x => new IssueCommentDto(x)).ToList()
        };
    }

    [Action("Create issue comment", Description = "Create an issue comment on a pull request")]
    public async Task<IssueCommentDto> CreateIssueComment(
        [ActionParameter] GetRepositoryRequest repositoryRequest,
        [ActionParameter] PullRequestIdentifierRequest identifier,
        [ActionParameter] CreateIssueCommentRequest input)
    {
        var repository = await GetRepositoryAsync(repositoryRequest.RepositoryId);
        var request = CreateRestRequest($"/{repository.Owner.Login}/{repository.Name}/issues/{identifier.PullRequestNumber}/comments", Method.Post);
        request.AddJsonBody(new
        {
            body = input.Body
        });

        var comment = await ClientRest.ExecuteWithErrorHandling<IssueCommentPayload>(request);
        return new(comment);
    }

    [Action("Update issue comment", Description = "Update an existing issue comment")]
    public async Task<IssueCommentDto> UpdateIssueComment(
        [ActionParameter] GetRepositoryRequest repositoryRequest,
        [ActionParameter] CommentIdentifierRequest identifier,
        [ActionParameter] UpdateIssueCommentRequest input)
    {
        var repository = await GetRepositoryAsync(repositoryRequest.RepositoryId);
        var request = CreateRestRequest($"/{repository.Owner.Login}/{repository.Name}/issues/comments/{identifier.CommentId}", Method.Patch);
        request.AddJsonBody(new
        {
            body = input.Body
        });

        var comment = await ClientRest.ExecuteWithErrorHandling<IssueCommentPayload>(request);
        return new(comment);
    }

    [Action("List pull request review comments", Description = "List inline review comments for a pull request")]
    public async Task<ListPullRequestReviewCommentsResponse> ListPullRequestReviewComments(
        [ActionParameter] GetRepositoryRequest repositoryRequest,
        [ActionParameter] PullRequestIdentifierRequest input)
    {
        var repository = await GetRepositoryAsync(repositoryRequest.RepositoryId);
        var request = CreateRestRequest($"/{repository.Owner.Login}/{repository.Name}/pulls/{input.PullRequestNumber}/comments", Method.Get);
        var comments = await ClientRest.ExecuteWithErrorHandling<List<PullRequestReviewCommentPayload>>(request);

        return new()
        {
            Comments = comments.Select(x => new PullRequestReviewCommentDto(x)).ToList()
        };
    }

    [Action("Create pull request review", Description = "Create a pull request review with optional inline comments")]
    public async Task<PullRequestReviewDto> CreatePullRequestReview(
        [ActionParameter] GetRepositoryRequest repositoryRequest,
        [ActionParameter] PullRequestIdentifierRequest identifier,
        [ActionParameter] CreatePullRequestReviewRequest input)
    {
        var repository = await GetRepositoryAsync(repositoryRequest.RepositoryId);
        var request = CreateRestRequest($"/{repository.Owner.Login}/{repository.Name}/pulls/{identifier.PullRequestNumber}/reviews", Method.Post);

        var comments = ParseReviewCommentsJson(input.CommentsJson);
        var body = new Dictionary<string, object?>
        {
            ["commit_id"] = input.CommitId,
            ["event"] = string.IsNullOrWhiteSpace(input.Event) ? "COMMENT" : input.Event,
            ["body"] = input.Body
        };

        if (comments.Count != 0)
        {
            body["comments"] = comments.Select(MapReviewComment).ToList();
        }

        request.AddJsonBody(body);

        var review = await ClientRest.ExecuteWithErrorHandling<PullRequestReviewPayload>(request);
        return new(review);
    }

    [Action("Compare commits", Description = "Compare two references and return the changed commits and files")]
    public async Task<CompareCommitsDto> CompareCommits(
        [ActionParameter] GetRepositoryRequest repositoryRequest,
        [ActionParameter] CompareCommitsRequest input)
    {
        var repository = await GetRepositoryAsync(repositoryRequest.RepositoryId);
        var request = CreateRestRequest($"/{repository.Owner.Login}/{repository.Name}/compare/{Uri.EscapeDataString(input.BaseReference)}...{Uri.EscapeDataString(input.HeadReference)}", Method.Get);
        var compare = await ClientRest.ExecuteWithErrorHandling<CompareCommitsPayload>(request);
        return new(compare);
    }

    private RestRequest CreateRestRequest(string resource, Method method)
    {
        var request = new RestRequest(resource, method);
        request.AddHeader("Accept", "application/vnd.github+json");
        request.AddHeader("X-GitHub-Api-Version", GithubApiVersion);
        return request;
    }

    private static int ParsePullRequestNumber(string pullRequestNumber)
    {
        if (!int.TryParse(pullRequestNumber, out var parsedNumber))
        {
            throw new PluginMisconfigurationException("Pull request number must be a valid integer.");
        }

        return parsedNumber;
    }

    private static List<PullRequestReviewCommentInput> ParseReviewCommentsJson(string? commentsJson)
    {
        if (string.IsNullOrWhiteSpace(commentsJson))
        {
            return [];
        }

        try
        {
            var comments = JsonConvert.DeserializeObject<List<PullRequestReviewCommentInput>>(commentsJson);
            return comments ?? [];
        }
        catch (JsonException ex)
        {
            throw new PluginMisconfigurationException($"Comments JSON is invalid: {ex.Message}");
        }
    }

    private static object MapReviewComment(PullRequestReviewCommentInput comment)
    {
        if (string.IsNullOrWhiteSpace(comment.Path))
        {
            throw new PluginMisconfigurationException("Each review comment must include 'path'.");
        }

        if (comment.Line is null)
        {
            throw new PluginMisconfigurationException("Each review comment must include 'line'.");
        }

        if (string.IsNullOrWhiteSpace(comment.Body))
        {
            throw new PluginMisconfigurationException("Each review comment must include 'body'.");
        }

        var result = new Dictionary<string, object?>
        {
            ["path"] = comment.Path,
            ["line"] = comment.Line,
            ["side"] = string.IsNullOrWhiteSpace(comment.Side) ? "RIGHT" : comment.Side,
            ["body"] = comment.Body
        };

        if (comment.StartLine.HasValue)
        {
            result["start_line"] = comment.StartLine.Value;
            result["start_side"] = string.IsNullOrWhiteSpace(comment.StartSide) ? "RIGHT" : comment.StartSide;
        }

        return result;
    }
}
