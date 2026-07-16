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
using Newtonsoft.Json.Linq;
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

    [Action("Search files in pull request", Description = "Search files changed in a pull request")]
    public async Task<SearchPullRequestFilesResponse> SearchPullRequestFiles(
        [ActionParameter] GetRepositoryRequest repositoryRequest,
        [ActionParameter] PullRequestIdentifierRequest input)
    {
        var files = await ExecuteWithErrorHandlingAsync(async () =>
            await ClientSdk.PullRequest.Files(long.Parse(repositoryRequest.RepositoryId), ParsePullRequestNumber(input.PullRequestNumber)));

        var fileDtos = files.Select(x => new PullRequestFileDto(x)).ToList();

        return new()
        {
            Files = fileDtos,
            FilesJson = JsonConvert.SerializeObject(fileDtos)
        };
    }

    [Action("Search commits in pull request", Description = "Search commits included in a pull request")]
    public async Task<SearchPullRequestCommitsResponse> SearchPullRequestCommits(
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

    [Action("Search issue comments in pull request", Description = "Search issue comments in a pull request")]
    public async Task<SearchIssueCommentsResponse> SearchIssueComments(
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
        var body = new
        {
            body = input.Body
        };

        request.AddJsonBody(body);
        var response = await ClientRest.ExecuteAsync(request);

        if (!response.IsSuccessful)
        {
            var error = TryParseGithubError(response.Content);
            throw new PluginApplicationException(error ?? response.ErrorMessage ?? "GitHub did not accept the issue comment request.");
        }

        return ParseIssueCommentResponse(response.Content, input.Body);
    }

    [Action("Update issue comment", Description = "Update an existing issue comment")]
    public async Task<IssueCommentDto> UpdateIssueComment(
        [ActionParameter] GetRepositoryRequest repositoryRequest,
        [ActionParameter] CommentIdentifierRequest identifier,
        [ActionParameter] UpdateIssueCommentRequest input)
    {
        var repository = await GetRepositoryAsync(repositoryRequest.RepositoryId);
        var request = CreateRestRequest($"/{repository.Owner.Login}/{repository.Name}/issues/comments/{identifier.CommentId}", Method.Patch);
        var body = new
        {
            body = input.Body
        };

        request.AddJsonBody(body);
        var response = await ClientRest.ExecuteAsync(request);

        if (!response.IsSuccessful)
        {
            var error = TryParseGithubError(response.Content);
            throw new PluginApplicationException(error ?? response.ErrorMessage ?? "GitHub did not accept the issue comment update request.");
        }

        return ParseIssueCommentResponse(response.Content, input.Body);
    }

    [Action("Search review comments in pull request", Description = "Search inline review comments in a pull request")]
    public async Task<SearchPullRequestReviewCommentsResponse> SearchPullRequestReviewComments(
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
        return await CreatePullRequestReviewInternal(repositoryRequest, identifier, input);
    }

    private async Task<PullRequestReviewDto> CreatePullRequestReviewInternal(
        GetRepositoryRequest repositoryRequest,
        PullRequestIdentifierRequest identifier,
        CreatePullRequestReviewRequest input)
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
        var response = await ClientRest.ExecuteAsync(request);

        if (!response.IsSuccessful)
        {
            var error = TryParseGithubError(response.Content);
            throw new PluginApplicationException(error ?? response.ErrorMessage ?? "GitHub did not accept the pull request review request.");
        }

        return ParsePullRequestReviewResponse(response.Content, input);
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

    private static PullRequestReviewDto ParsePullRequestReviewResponse(string? content, CreatePullRequestReviewRequest input)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            return new PullRequestReviewDto
            {
                Body = input.Body ?? string.Empty,
                CommitId = input.CommitId ?? string.Empty,
                State = string.IsNullOrWhiteSpace(input.Event) ? "COMMENT" : input.Event
            };
        }

        try
        {
            var json = JObject.Parse(content);
            return new PullRequestReviewDto
            {
                Id = json["id"]?.ToString() ?? string.Empty,
                NodeId = json["node_id"]?.ToString() ?? string.Empty,
                Body = json["body"]?.ToString() ?? input.Body ?? string.Empty,
                State = json["state"]?.ToString() ?? (string.IsNullOrWhiteSpace(input.Event) ? "COMMENT" : input.Event),
                CommitId = json["commit_id"]?.ToString() ?? input.CommitId ?? string.Empty,
                HtmlUrl = json["html_url"]?.ToString() ?? string.Empty,
                UserLogin = json["user"]?["login"]?.ToString() ?? string.Empty,
                SubmittedAt = DateTimeOffset.TryParse(json["submitted_at"]?.ToString(), out var submittedAt)
                    ? submittedAt.UtcDateTime
                    : null
            };
        }
        catch
        {
            return new PullRequestReviewDto
            {
                Body = input.Body ?? string.Empty,
                CommitId = input.CommitId ?? string.Empty,
                State = string.IsNullOrWhiteSpace(input.Event) ? "COMMENT" : input.Event
            };
        }
    }

    private static IssueCommentDto ParseIssueCommentResponse(string? content, string? fallbackBody)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            return new IssueCommentDto(new IssueCommentPayload
            {
                Body = fallbackBody
            });
        }

        try
        {
            var payload = JsonConvert.DeserializeObject<IssueCommentPayload>(content);
            if (payload is not null)
            {
                return new IssueCommentDto(payload);
            }
        }
        catch
        {
            // Fall back to lenient parsing below.
        }

        try
        {
            var json = JObject.Parse(content);
            return new IssueCommentDto(new IssueCommentPayload
            {
                Id = long.TryParse(json["id"]?.ToString(), out var id) ? id : 0,
                NodeId = json["node_id"]?.ToString(),
                Body = json["body"]?.ToString() ?? fallbackBody,
                HtmlUrl = json["html_url"]?.ToString(),
                User = new GithubUserPayload
                {
                    Login = json["user"]?["login"]?.ToString()
                },
                CreatedAt = DateTimeOffset.TryParse(json["created_at"]?.ToString(), out var createdAt)
                    ? createdAt
                    : null,
                UpdatedAt = DateTimeOffset.TryParse(json["updated_at"]?.ToString(), out var updatedAt)
                    ? updatedAt
                    : null
            });
        }
        catch
        {
            return new IssueCommentDto(new IssueCommentPayload
            {
                Body = fallbackBody
            });
        }
    }

    private static string? TryParseGithubError(string? content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            return null;
        }

        try
        {
            var error = JsonConvert.DeserializeObject<RestErrorDto>(content);
            return error?.Message;
        }
        catch
        {
            return null;
        }
    }
}
