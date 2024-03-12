using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Apps.Github.Dtos;
using Blackbird.Applications.Sdk.Common.Actions;
using Apps.Github.Models.Respository.Responses;
using Apps.Github.Models.Respository.Requests;
using System.Net.Mime;
using Apps.GitHub;
using Apps.Github.Actions.Base;
using Apps.Github.Models.Commit.Responses;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using Octokit;
using RepositoryRequest = Apps.Github.Models.Respository.Requests.RepositoryRequest;
using Apps.GitHub.Models.Branch.Requests;

namespace Apps.Github.Actions;

[ActionList]
public class RepositoryActions : GithubActions
{
    private readonly IFileManagementClient _fileManagementClient;
    
    public RepositoryActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient) 
        : base(invocationContext)
    {
        _fileManagementClient = fileManagementClient;
    }

    [Action("Create new repository", Description = "Create new repository")]
    public RepositoryDto CreateRepository([ActionParameter] CreateRepositoryRequest input)
    {
        var repository = Client.Repository.Create(input.GetNewRepositoryRequest()).Result;
        return new RepositoryDto(repository);
    }

    [Action("Get repository file", Description = "Get repository file by path")]
    public GetFileResponse GetFile([ActionParameter] GetFileRequest input)
    {
        var repoInfo = GetRepositoryById(new() { RepositoryId = input.RepositoryId });
        var fileData = Client.Repository.Content
            .GetRawContent(repoInfo.OwnerLogin, repoInfo.Name, input.FilePath).Result;

        var filename = Path.GetFileName(input.FilePath);
        if (!MimeTypes.TryGetMimeType(filename, out var mimeType))
            mimeType = MediaTypeNames.Application.Octet;

        FileReference file;
        using (var stream = new MemoryStream(fileData))
        {
            file = _fileManagementClient.UploadAsync(stream, mimeType, filename).Result;
        }
        
        return new GetFileResponse
        {
            FilePath = input.FilePath,
            File = file,
            FileExtension = Path.GetExtension(input.FilePath)
        };
    }

    [Action("Get all files in folder", Description = "Get all files in folder")]
    public GetRepositoryFilesFromFilepathsResponse GetAllFilesInFolder(
        [ActionParameter] GetAllFilesInFolderRequest input)
    {
        var resultFiles = new List<GithubFile>();
        var content = ListRepositoryContent(new RepositoryContentRequest
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
                File = fileData.File,
                FilePath = fileData.FilePath
            });
        }

        return new GetRepositoryFilesFromFilepathsResponse { Files = resultFiles };
    }

    [Action("Get repository", Description = "Get repository info")]
    public RepositoryDto GetRepositoryById([ActionParameter] GetRepositoryRequest input)
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
    public RepositoryContentResponse ListRepositoryContent([ActionParameter] RepositoryContentRequest input)
    {
        var content = Client.Repository.Content.GetAllContents(long.Parse(input.RepositoryId), input.Path ?? "/")
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
        [ActionParameter] GetRepositoryRequest repositoryRequest,
        [ActionParameter] GetOptionalBranchRequest branchRequest)
    {
        var commits = new CommitActions(InvocationContext, _fileManagementClient)
            .ListRepositoryCommits(repositoryRequest, branchRequest);
        var tree = Client.Git.Tree.GetRecursive(long.Parse(repositoryRequest.RepositoryId), commits.Commits.First().Id)
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
                File = fileData.File
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