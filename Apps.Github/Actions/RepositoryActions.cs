using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Apps.Github.Dtos;
using Blackbird.Applications.Sdk.Common.Actions;
using Apps.Github.Models.Respository.Responses;
using Apps.Github.Models.Respository.Requests;
using File = Blackbird.Applications.Sdk.Common.Files.File;
using System.Net.Mime;
using Apps.Github.Actions.Base;
using Apps.Github.Models.Commit.Responses;
using Blackbird.Applications.Sdk.Common.Invocation;
using Octokit;
using RepositoryRequest = Apps.Github.Models.Respository.Requests.RepositoryRequest;

namespace Apps.Github.Actions;

[ActionList]
public class RepositoryActions : GithubActions
{
    public RepositoryActions(InvocationContext invocationContext) : base(invocationContext)
    {
    }

    [Action("Get repository file", Description = "Get repository file by path")]
    public GetFileResponse GetFile([ActionParameter] GetFileRequest input)
    {
        var repoInfo = GetRepositoryById(new() { RepositoryId = input.RepositoryId });
        var fileData = Client.Repository.Content
            .GetRawContent(repoInfo.OwnerLogin, repoInfo.Name, input.FilePath).Result;

        string filename = Path.GetFileName(input.FilePath);
        if (!MimeTypes.TryGetMimeType(filename, out var mimeType))
            mimeType = MediaTypeNames.Application.Octet;

        return new GetFileResponse
        {
            FilePath = input.FilePath,
            File = new File(fileData)
            {
                ContentType = mimeType,
                Name = filename
            },
            FileExtension = Path.GetExtension(input.FilePath)
        };
    }

    [Action("Get all files in folder", Description = "Get all files in folder")]
    public GetRepositoryFilesFromFilepathsResponse GetAllFilesInFolder(
        [ActionParameter] GetAllFilesInFolderRequest input)
    {
        var resultFiles = new List<GithubFile>();
        var content = ListRepositoryContent(Creds, new RepositoryContentRequest
        {
            Path = input.FolderPath,
            RepositoryId = input.RepositoryId,
        });
        foreach (var file in content.Content)
        {
            var fileData = GetFile(new()
            {
                FilePath = file.Path,
                RepositoryId = input.RepositoryId
            });
            
            resultFiles.Add(new GithubFile()
            {
                File = new(fileData.File.Bytes)
                {
                    Name = fileData.File.Name,
                    ContentType = fileData.File.ContentType
                },
                FilePath = fileData.FilePath
            });
        }

        return new GetRepositoryFilesFromFilepathsResponse { Files = resultFiles };
    }

    [Action("Get repository by name", Description = "Get repository info by owner and name")]
    public RepositoryDto GetRepositoryByName([ActionParameter] GetRepositoryByNameRequest input)
    {
        var repository = Client.Repository.Get(input.RepositoryOwner, input.RepositoryName).Result;
        return new RepositoryDto(repository);
    }

    [Action("Get repository by id", Description = "Get repository info by id")]
    public RepositoryDto GetRepositoryById([ActionParameter] GetRepositoryByIdRequest input)
    {
        var repository = Client.Repository.Get(long.Parse(input.RepositoryId)).Result;
        return new RepositoryDto(repository);
    }

    [Action("Get repository issues", Description = "Get opened issues against repository")]
    public GetIssuesResponse GetIssuesInRepository([ActionParameter] RepositoryRequest input)
    {
        var issues = Client.Issue.GetAllForRepository(long.Parse(input.RepositoryId)).Result;

        return new()
        {
            Issues = issues.Select(issue => new IssueDto(issue))
        };
    }

    [Action("Get repository pull requests", Description = "Get opened pull requests in a repository")]
    public GetPullRequestsResponse GetPullRequestsInRepository([ActionParameter] RepositoryRequest input)
    {
        var pullRequests = Client.PullRequest.GetAllForRepository(long.Parse(input.RepositoryId)).Result;
        return new()
        {
            PullRequests = pullRequests.Select(p => new PullRequestDto(p))
        };
    }

    [Action("List repository folder content", Description = "List repository content by specified path")]
    public RepositoryContentResponse ListRepositoryContent(
        IEnumerable<AuthenticationCredentialsProvider> Creds,
        [ActionParameter] RepositoryContentRequest input)
    {
        var content = Client.Repository.Content.GetAllContents(long.Parse(input.RepositoryId), input.Path)
            .Result;
        return new()
        {
            Content = content
        };
    }

    [Action("List repositories", Description = "List all repositories")]
    public async Task<ListRepositoriesResponse> ListRepositories()
    {
        var content = await Client.Repository.GetAllForCurrent();
        var repositories = content.Select(x => new RepositoryDto(x)).ToArray();

        return new(repositories);
    }

    [Action("List all repository content", Description = "List all repository content (paths)")]
    public RepositoryContentPathsResponse ListAllRepositoryContent(
        [ActionParameter] ListAllRepositoryContentRequest input)
    {
        var commits = new CommitActions(InvocationContext)
            .ListRepositoryCommits(new() { RepositoryId = input.RepositoryId });
        var tree = Client.Git.Tree.GetRecursive(long.Parse(input.RepositoryId), commits.Commits.First().Id)
            .Result;
        var paths = tree.Tree.Select(x => new RepositoryItem
        {
            Sha = x.Sha,
            Path = x.Path,
            IsFolder = x.Type == TreeType.Tree
        });
        return new RepositoryContentPathsResponse
        {
            Items = paths
        };
    }

    [Action("Get files by filepaths", Description = "Get files by filepaths from webhooks")]
    public GetRepositoryFilesFromFilepathsResponse GetRepositoryFilesFromFilepaths(
        [ActionParameter] GetRepositoryFilesFromFilepathsRequest input)
    {
        var files = new List<GithubFile>();
        foreach (var filePath in input.Files)
        {
            var fileData = GetFile(new GetFileRequest
            {
                RepositoryId = input.RepositoryId,
                FilePath = filePath
            });
            files.Add(new GithubFile
            {
                FilePath = fileData.FilePath,
                File = new(fileData.File.Bytes)
                {
                    Name = fileData.File.Name,
                    ContentType = fileData.File.ContentType
                }
            });
        }

        return new()
        {
            Files = files
        };
    }

    [Action("Is file in folder", Description = "Is file in folder")]
    public IsFileInFolderResponse IsFileInFolder([ActionParameter] IsFileInFolderRequest input)
    {
        return new()
        {
            IsFileInFolder = input.FilePath.Split('/').SkipLast(1).Contains(input.FolderName) ? 1 : 0
        };
    }
}