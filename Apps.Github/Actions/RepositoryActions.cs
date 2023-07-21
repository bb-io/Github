using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Apps.Github.Dtos;
using Blackbird.Applications.Sdk.Common.Actions;
using Apps.Github.Models.Respository.Responses;
using Apps.Github.Models.Responses;
using Apps.Github.Models.Requests;
using Apps.Github.Models.Respository.Requests;
using Apps.Github.Models.Commit.Requests;

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
            var repoInfo = GetRepositoryById(authenticationCredentialsProviders, new GetRepositoryByIdRequest() { RepositoryId = input.RepositoryId });
            var fileData = githubClient.Repository.Content.GetRawContent(repoInfo.OwnerLogin, repoInfo.Name, input.FilePath).Result;
            return new GetFileResponse()
            {
                FileName = Path.GetFileName(input.FilePath),
                File = fileData,
                FilePath = input.FilePath,
                FileExtension = Path.GetExtension(input.FilePath)
            };
        }

        [Action("Get all files in folder", Description = "Get all files in folder")]
        public GetRepositoryFilesFromFilepathsResponse GetAllFilesInFolder(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] GetAllFilesInFolderRequest input)
        {
            var resultFiles = new List<FileData>();
            var content = ListRepositoryContent(authenticationCredentialsProviders, new RepositoryContentRequest()
            {
                Path = input.FolderPath,
                RepositoryId = input.RepositoryId,
            });
            foreach (var file in content.Content)
            {
                var fileData = GetFile(authenticationCredentialsProviders, new GetFileRequest() { FilePath = file.Path, RepositoryId = input.RepositoryId });
                resultFiles.Add(new FileData() { Filename = fileData.FileName, File = fileData.File });
            }
            return new GetRepositoryFilesFromFilepathsResponse() { Files = resultFiles };
        }

        [Action("Get repository by name", Description = "Get repository info by owner and name")]
        public RepositoryDto GetRepositoryByName(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] GetRepositoryByNameRequest input)
        {
            var githubClient = new BlackbirdGithubClient(authenticationCredentialsProviders);
            var repository = githubClient.Repository.Get(input.RepositoryOwner, input.RepositoryName).Result;
            return new RepositoryDto(repository);
        }

        [Action("Get repository by id", Description = "Get repository info by id")]
        public RepositoryDto GetRepositoryById(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] GetRepositoryByIdRequest input)
        {
            var githubClient = new BlackbirdGithubClient(authenticationCredentialsProviders);
            var repository = githubClient.Repository.Get(long.Parse(input.RepositoryId)).Result;
            return new RepositoryDto(repository);
        }

        [Action("Get repository issues", Description = "Get opened issues against repository")]
        public GetIssuesResponse GetIssuesInRepository(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] Models.Requests.RepositoryRequest input)
        {
            var githubClient = new BlackbirdGithubClient(authenticationCredentialsProviders);
            var issues = githubClient.Issue.GetAllForRepository(long.Parse(input.RepositoryId)).Result;
            return new GetIssuesResponse()
            {
                Issues = issues.Select(issue => new IssueDto(issue))
            };
        }

        [Action("Get repository pull requests", Description = "Get opened pull requests in a repository")]
        public GetPullRequestsResponse GetPullRequestsInRepository(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] Models.Requests.RepositoryRequest input)
        {
            var githubClient = new BlackbirdGithubClient(authenticationCredentialsProviders);
            var pullRequests = githubClient.PullRequest.GetAllForRepository(long.Parse(input.RepositoryId)).Result;
            return new GetPullRequestsResponse()
            {
                PullRequests = pullRequests.Select(p => new PullRequestDto(p))
            };
        }

        [Action("List repository folder content", Description = "List repository content by specified path")]
        public RepositoryContentResponse ListRepositoryContent(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] RepositoryContentRequest input)
        {
            var githubClient = new BlackbirdGithubClient(authenticationCredentialsProviders);
            var content = githubClient.Repository.Content.GetAllContents(long.Parse(input.RepositoryId), input.Path).Result;
            return new RepositoryContentResponse()
            {
                Content = content
            };
        }

        [Action("List all repository content", Description = "List all repository content (paths)")]
        public RepositoryContentPathsResponse ListAllRepositoryContent(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] ListAllRepositoryContentRequest input)
        {
            var githubClient = new BlackbirdGithubClient(authenticationCredentialsProviders);
            var content = githubClient.Repository.Content.GetAllContents(long.Parse(input.RepositoryId)).Result;

            var commits = new CommitActions().ListRepositoryCommits(authenticationCredentialsProviders, 
                new ListRepositoryCommitsRequest() { RepositoryId = input.RepositoryId });
            var tree = githubClient.Git.Tree.GetRecursive(long.Parse(input.RepositoryId), commits.Commits.First().Id).Result;
            var paths = tree.Tree.Select(x => new RepositoryItem()
            {
                Sha = x.Sha,
                Path = x.Path,
            });
            return new RepositoryContentPathsResponse()
            {
                Items = paths
            };
        }

        [Action("Get files by filepaths", Description = "Get files by filepaths from webhooks")]
        public GetRepositoryFilesFromFilepathsResponse GetRepositoryFilesFromFilepaths(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] GetRepositoryFilesFromFilepathsRequest input)
        {
            var files = new List<FileData>();
            foreach(var filePath in input.Files)
            {
                var fileData = GetFile(authenticationCredentialsProviders, new GetFileRequest()
                {
                    RepositoryId = input.RepositoryId,
                    FilePath = filePath.FilePath
                });
                files.Add(new FileData()
                {
                    File = fileData.File,
                    Filename = fileData.FileName
                });
            }
            return new GetRepositoryFilesFromFilepathsResponse()
            {
                Files = files
            };
        }

        [Action("Is file in folder", Description = "Is file in folder")]
        public IsFileInFolderResponse IsFileInFolder(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] IsFileInFolderRequest input)
        {
            return new IsFileInFolderResponse()
            {
                IsFileInFolder = input.FilePath.Split('/').SkipLast(1).Contains(input.FolderName) ? 1 : 0
            };
        }
    }
}
