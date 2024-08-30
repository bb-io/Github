using Blackbird.Applications.Sdk.Common;
using Apps.Github.Dtos;
using Blackbird.Applications.Sdk.Common.Actions;
using Apps.Github.Models.Respository.Responses;
using Apps.Github.Models.Respository.Requests;
using System.Net.Mime;
using Apps.GitHub;
using Apps.Github.Actions.Base;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using RepositoryRequest = Apps.Github.Models.Respository.Requests.RepositoryRequest;
using Apps.GitHub.Models.Branch.Requests;
using Apps.GitHub.Models.Respository.Responses;

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

    //V2
    [Action("Download file", Description = "Download a file from a specified folder")]
    public async Task<FileReference> GetFile(
        [ActionParameter] GetRepositoryRequest repositoryRequest,
        [ActionParameter] GetOptionalBranchRequest branchRequest,
        [ActionParameter] DownloadFileRequest downloadFileRequest)
    {
        var repositoryInfo = await Client.Repository.Get(long.Parse(repositoryRequest.RepositoryId));
        var fileInfo = string.IsNullOrEmpty(branchRequest.Name)
            ? await Client.Repository.Content.GetAllContents(repositoryInfo.Owner.Login, repositoryInfo.Name, downloadFileRequest.FilePath)
            : await Client.Repository.Content.GetAllContentsByRef(repositoryInfo.Owner.Login, repositoryInfo.Name,
                downloadFileRequest.FilePath, branchRequest.Name);
        if (fileInfo == null)
            throw new ArgumentException($"File does not exist ({downloadFileRequest.FilePath})");

        var filename = Path.GetFileName(downloadFileRequest.FilePath);
        if (!MimeTypes.TryGetMimeType(filename, out var mimeType))
            mimeType = MediaTypeNames.Application.Octet;

        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, fileInfo.First().DownloadUrl);
        FileReference fileReference = new FileReference(httpRequestMessage, filename, mimeType);
        return fileReference;
    }

    //V2
    [Action("Search files in folder", Description = "Search files in folder")]
    public async Task<SearchFileInFolderResponse> SearchFilesInFolder(
        [ActionParameter] GetRepositoryRequest repositoryRequest,
        [ActionParameter] GetOptionalBranchRequest branchRequest,
        [ActionParameter] FolderContentRequest folderContentRequest)
    {      
        var folderContent = (string.IsNullOrEmpty(branchRequest.Name)
            ? await Client.Repository.Content.GetAllContents(long.Parse(repositoryRequest.RepositoryId),
                folderContentRequest.Path ?? "/")
            : await Client.Repository.Content.GetAllContentsByRef(long.Parse(repositoryRequest.RepositoryId),
                folderContentRequest.Path ?? "/", branchRequest.Name)).ToList();
        
        var result = new SearchFileInFolderResponse(folderContent, folderContentRequest.Filter);

        if (folderContentRequest.IncludeSubfolders.HasValue && folderContentRequest.IncludeSubfolders.Value)
        {
            foreach (var folder in folderContent.Where(x => x.Type.Value == Octokit.ContentType.Dir).ToList())
            {
                var innerContent = await SearchFilesInFolder(repositoryRequest, branchRequest,
                    new(folder.Path, true));
                result.Files.AddRange(innerContent.Files);
            }
        }
        return result;
    }


    [Action("Get repository issues", Description = "Get opened issues against repository")]
    public async Task<GetIssuesResponse> GetIssuesInRepository([ActionParameter] RepositoryRequest input)
    {
        var issues = await Client.Issue.GetAllForRepository(long.Parse(input.RepositoryId));

        return new()
        {
            Issues = issues.Select(issue => new IssueDto(issue))
        };
    }

    //[Action("Get repository pull requests", Description = "Get opened pull requests in a repository")]
    //public async Task<GetPullRequestsResponse> GetPullRequestsInRepository([ActionParameter] RepositoryRequest input)
    //{
    //    var pullRequests = await Client.PullRequest.GetAllForRepository(long.Parse(input.RepositoryId));
    //    return new()
    //    {
    //        PullRequests = pullRequests.Select(p => new PullRequestDto(p))
    //    };
    //}


    //[Action("List repositories", Description = "List all repositories")]
    //public async Task<ListRepositoriesResponse> ListRepositories()
    //{
    //    var content = await Client.Repository.GetAllForCurrent();
    //    var repositories = content.Select(x => new RepositoryDto(x)).ToArray();

    //    return new(repositories);
    //}

    // Remove this
    //[Action("List all repository content", Description = "List all repository content (paths)")]
    //public async Task<RepositoryContentPathsResponse> ListAllRepositoryContent(
    //    [ActionParameter] GetRepositoryRequest repositoryRequest,
    //    [ActionParameter] GetOptionalBranchRequest branchRequest)
    //{
    //    var commits = await new CommitActions(InvocationContext, _fileManagementClient)
    //        .ListRepositoryCommits(repositoryRequest, branchRequest);

    //    TreeResponse tree = null;
    //    foreach(var commit in commits.Commits)
    //    {
    //        try
    //        {
    //            tree = await Client.Git.Tree.GetRecursive(long.Parse(repositoryRequest.RepositoryId), commit.Id);
    //            break;
    //        }
    //        catch (Octokit.NotFoundException)
    //        {}
    //    }
        
    //    var paths = tree.Tree.Select(x => new RepositoryItem
    //    {
    //        Sha = x.Sha,
    //        Path = x.Path,
    //        IsFolder = x.Type == TreeType.Tree
    //    });
    //    return new()
    //    {
    //        Items = paths
    //    };
    //}

    //[Action("List all repository folders", Description = "List all repository folders")]
    //public async Task<RepositoryContentPathsResponse> ListAllRepositoryFolder(
    //    [ActionParameter] GetRepositoryRequest repositoryRequest,
    //    [ActionParameter] GetOptionalBranchRequest branchRequest)
    //{
    //    var commits = await new CommitActions(InvocationContext, _fileManagementClient)
    //        .ListRepositoryCommits(repositoryRequest, branchRequest);
    //    var tree = await Client.Git.Tree.GetRecursive(long.Parse(repositoryRequest.RepositoryId),
    //        commits.Commits.First().Id);
    //    var paths = tree.Tree.Where(x => x.Type == TreeType.Tree).Select(x => new RepositoryItem
    //    {
    //        Sha = x.Sha,
    //        Path = x.Path,
    //        IsFolder = x.Type == TreeType.Tree
    //    });
    //    return new()
    //    {
    //        Items = paths
    //    };
    //}

    //[Action("Get files by filepaths", Description = "Get files by filepaths from webhooks")]
    //public async Task<GetRepositoryFilesFromFilepathsResponse> GetRepositoryFilesFromFilepaths(
    //    [ActionParameter] GetRepositoryRequest repositoryRequest,
    //    [ActionParameter] GetOptionalBranchRequest branchRequest,
    //    [ActionParameter] GetRepositoryFilesFromFilepathsRequest input)
    //{
    //    var files = new List<GithubFile>();
    //    foreach (var filePath in input.FilePaths)
    //    {
    //        var fileData = await GetFile(
    //            repositoryRequest,
    //            branchRequest,
    //            new() { FilePath = filePath });

    //        files.Add(new()
    //        {
    //            FilePath = fileData.FilePath,
    //            File = fileData.File
    //        });
    //    }

    //    return new()
    //    {
    //        Files = files
    //    };
    //}
    
    [Action("File exists", Description = "Check if file exists in repository")]
    public async Task<bool> FileExists(
        [ActionParameter] GetRepositoryRequest repositoryRequest,
        [ActionParameter] GetOptionalBranchRequest branchRequest,
        [ActionParameter] DownloadFileRequest getFileRequest)
    {
        try
        {
            var repoInfo = await Client.Repository.Get(long.Parse(repositoryRequest.RepositoryId));
            var fileData = string.IsNullOrEmpty(branchRequest.Name)
                ? await Client.Repository.Content.GetRawContent(repoInfo.Owner.Login, repoInfo.Name, getFileRequest.FilePath)
                : await Client.Repository.Content.GetRawContentByRef(repoInfo.Owner.Login, repoInfo.Name,
                    getFileRequest.FilePath, branchRequest.Name);

            if (fileData == null)
            {
                return false;
            }
        
            return true;
        }
        catch (Exception e)
        {
            if(e.Message.Contains("Not Found"))
            {
                return false;
            }
            
            throw;
        }
    }

    [Action("Debug", Description = "Debug")]
    public string Debug()
    {
        return InvocationContext.AuthenticationCredentialsProviders.First(p => p.KeyName == "Authorization").Value;
    }
}