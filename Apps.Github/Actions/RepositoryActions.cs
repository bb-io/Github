using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Apps.Github.Models.Requests;
using Apps.Github.Models.Responses;
using Octokit;
using Apps.Github.Dtos;
using RepositoryRequest = Apps.Github.Models.Requests.RepositoryRequest;
using Blackbird.Applications.Sdk.Common.Actions;

namespace Apps.Github.Actions
{
    [ActionList]
    public class RepositoryActions
    {
        [Action("Get repository file", Description = "Get repository file by path")]
        public GetFileResponse GetFile(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] GetFileRequest input)
        {
            var githubClient = new BlackbirdGithubClient(authenticationCredentialsProviders);
            var fileData = githubClient.Repository.Content.GetRawContent(input.RepositoryOwnerLogin, input.RepositoryName, input.FilePath).Result;
            return new GetFileResponse()
            {
                FileName = Path.GetFileName(input.FilePath),
                File = fileData
            };
        }

        [Action("Get repository issues", Description = "Get opened issues against repository")]
        public GetIssuesResponse GetIssuesInRepository(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] RepositoryRequest input)
        {
            var githubClient = new BlackbirdGithubClient(authenticationCredentialsProviders);
            var issues = githubClient.Issue.GetAllForRepository(input.RepositoryOwnerLogin, input.RepositoryName).Result;
            var response = new List<IssueDto>();
            foreach (var issue in issues)
            {
                response.Add(new IssueDto()
                {
                    Title = issue.Title,
                    Body = issue.Body,
                    UserLogin = issue.User.Login,
                    Url = issue.HtmlUrl,
                });
            }
            return new GetIssuesResponse()
            {
                Issues = response
            };
        }

        [Action("Get repository pull requests", Description = "Get opened pull requests in a repository")]
        public GetPullRequestsResponse GetPullRequestsInRepository(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] RepositoryRequest input)
        {
            var githubClient = new BlackbirdGithubClient(authenticationCredentialsProviders);
            var pullRequests = githubClient.PullRequest.GetAllForRepository(input.RepositoryOwnerLogin, input.RepositoryName).Result;
            var response = new List<PullRequestDto>();
            foreach (var pullRequest in pullRequests)
            {
                response.Add(new PullRequestDto()
                {
                    Title = pullRequest.Title,
                    Body = pullRequest.Body,
                    UserLogin = pullRequest.User.Login,
                    Url = pullRequest.HtmlUrl,
                });
            }
            return new GetPullRequestsResponse()
            {
                PullRequests = response
            };
        }

        [Action("List repository content", Description = "List repository content by specified path")]
        public RepositoryContentResponse ListRepositoryContent(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] RepositoryContentRequest input)
        {
            var githubClient = new BlackbirdGithubClient(authenticationCredentialsProviders);
            var content = githubClient.Repository.Content.GetAllContents(input.RepositoryOwnerLogin, input.RepositoryName, input.Path).Result;
            var response = new List<string>();
            foreach (var item in content)
            {
                response.Add(item.Name);
            }
            return new RepositoryContentResponse()
            {
                Content = response
            };
        }
    }
}
