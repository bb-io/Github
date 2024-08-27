using Blackbird.Applications.Sdk.Common;
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
using Blackbird.Applications.Sdk.Utils.Extensions.Files;
using Blackbird.Applications.Sdk.Utils.Models;
using Blackbird.Applications.Sdk.Common.Authentication;

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
    public async Task<RepositoryDto> CreateRepository([ActionParameter] CreateRepositoryRequest input)
    {
        var repository = await Client.Repository.Create(input.GetNewRepositoryRequest());
        return new(repository);
    }

    // Add large file downloading using download_url parameter
    [Action("Download file", Description = "Download a file from a specified folder")]
    public async Task<GetFileResponse> GetFile(
        [ActionParameter] GetRepositoryRequest repositoryRequest,
        [ActionParameter] GetOptionalBranchRequest branchRequest,
        [ActionParameter] GetFileRequest getFileRequest)
    {
        var repoInfo = await GetRepositoryById(repositoryRequest);
        var fileData = string.IsNullOrEmpty(branchRequest.Name)
            ? await Client.Repository.Content.GetRawContent(repoInfo.OwnerLogin, repoInfo.Name, getFileRequest.FilePath)
            : await Client.Repository.Content.GetRawContentByRef(repoInfo.OwnerLogin, repoInfo.Name,
                getFileRequest.FilePath, branchRequest.Name);
        if (fileData == null)
            throw new ArgumentException($"File does not exist ({getFileRequest.FilePath})");

        var filename = Path.GetFileName(getFileRequest.FilePath);
        if (!MimeTypes.TryGetMimeType(filename, out var mimeType))
            mimeType = MediaTypeNames.Application.Octet;

        FileReference file;
        using (var stream = new MemoryStream(fileData))
        {
            file = await _fileManagementClient.UploadAsync(stream, mimeType, filename);
        }

        return new()
        {
            FilePath = getFileRequest.FilePath,
            File = file,
            FileExtension = Path.GetExtension(getFileRequest.FilePath)
        };
    }

    // Change to:
    // Search files in folder
        // Recursive optional
        // name + * searching (*.html)
    // Return the paths
    [Action("Get all files in folder", Description = "Get all files in folder")]
    public async Task<GetRepositoryFilesFromFilepathsResponse> GetAllFilesInFolder(
        [ActionParameter] GetRepositoryRequest repositoryRequest,
        [ActionParameter] GetOptionalBranchRequest branchRequest,
        [ActionParameter] FolderContentRequest folderContentRequest)
    {
        var resultFiles = new List<GithubFile>();
        var content = string.IsNullOrEmpty(branchRequest.Name)
            ? await Client.Repository.Content.GetArchive(long.Parse(repositoryRequest.RepositoryId),
                ArchiveFormat.Zipball)
            : await Client.Repository.Content.GetArchive(long.Parse(repositoryRequest.RepositoryId),
                ArchiveFormat.Zipball, branchRequest.Name);
        if (content == null || content.Length == 0)
        {
            throw new ArgumentException("Repository is empty!");
        }

        var filesFromZip = new List<BlackbirdZipEntry>();
        using (var stream = new MemoryStream(content))
        {
            filesFromZip = (await stream.GetFilesFromZip()).ToList();
        }

        foreach (var file in filesFromZip)
        {
            file.Path = file.Path.Substring(file.Path.IndexOf('/') + 1);
            var includeSubFolders = folderContentRequest.IncludeSubfolders.HasValue &&
                                    folderContentRequest.IncludeSubfolders.Value;
            if (file.FileStream.Length == 0)
            {
                continue;
            }
            else if (!string.IsNullOrEmpty(folderContentRequest.Path))
            {
                if ((includeSubFolders && !file.Path.StartsWith(folderContentRequest.Path)) ||
                    (!includeSubFolders && Path.GetDirectoryName(file.Path).TrimStart('\\').Replace('\\', '/') !=
                        folderContentRequest.Path.Trim('/')))
                {
                    continue;
                }
            }
            else if (!includeSubFolders && !string.IsNullOrEmpty(Path.GetDirectoryName(file.Path)))
            {
                continue;
            }

            var filename = Path.GetFileName(file.Path);
            if (!MimeTypes.TryGetMimeType(filename, out var mimeType))
                mimeType = MediaTypeNames.Application.Octet;
            var uploadedFile = await _fileManagementClient.UploadAsync(file.FileStream, mimeType, filename);
            resultFiles.Add(new()
            {
                File = uploadedFile,
                FilePath = file.Path
            });
        }

        return new() { Files = resultFiles };
    }

    // [Action("Get repository", Description = "Get repository info")]
    public async Task<RepositoryDto> GetRepositoryById([ActionParameter] GetRepositoryRequest input)
    {
        var repository = await Client.Repository.Get(long.Parse(input.RepositoryId));
        return new(repository);
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

    // Remove this one
    [Action("List repository folder content", Description = "List repository content by specified path")]
    public async Task<RepositoryContentResponse> ListRepositoryContent(
        [ActionParameter] GetRepositoryRequest repositoryRequest,
        [ActionParameter] GetOptionalBranchRequest branchRequest,
        [ActionParameter] FolderContentRequest input)
    {
        List<RepositoryContent> content = (string.IsNullOrEmpty(branchRequest.Name)
            ? await Client.Repository.Content.GetAllContents(long.Parse(repositoryRequest.RepositoryId),
                input.Path ?? "/")
            : await Client.Repository.Content.GetAllContentsByRef(long.Parse(repositoryRequest.RepositoryId),
                input.Path ?? "/", branchRequest.Name)).ToList();
        if (input.IncludeSubfolders.HasValue && input.IncludeSubfolders.Value)
        {
            foreach (var folder in content.Where(x => x.Type.Value == Octokit.ContentType.Dir).ToList())
            {
                var innerContent = await ListRepositoryContent(repositoryRequest, branchRequest,
                    new($"{input.Path?.TrimEnd('/')}/{folder.Name}", true));
                content.AddRange(innerContent.Content);
            }
        }

        return new()
        {
            Content = content
        };
    }

    //[Action("List repositories", Description = "List all repositories")]
    //public async Task<ListRepositoriesResponse> ListRepositories()
    //{
    //    var content = await Client.Repository.GetAllForCurrent();
    //    var repositories = content.Select(x => new RepositoryDto(x)).ToArray();

    //    return new(repositories);
    //}

    // Remove this
    [Action("List all repository content", Description = "List all repository content (paths)")]
    public async Task<RepositoryContentPathsResponse> ListAllRepositoryContent(
        [ActionParameter] GetRepositoryRequest repositoryRequest,
        [ActionParameter] GetOptionalBranchRequest branchRequest)
    {
        var commits = await new CommitActions(InvocationContext, _fileManagementClient)
            .ListRepositoryCommits(repositoryRequest, branchRequest);

        TreeResponse tree = null;
        foreach(var commit in commits.Commits)
        {
            try
            {
                tree = await Client.Git.Tree.GetRecursive(long.Parse(repositoryRequest.RepositoryId), commit.Id);
                break;
            }
            catch (Octokit.NotFoundException)
            {}
        }
        
        var paths = tree.Tree.Select(x => new RepositoryItem
        {
            Sha = x.Sha,
            Path = x.Path,
            IsFolder = x.Type == TreeType.Tree
        });
        return new()
        {
            Items = paths
        };
    }

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

    // Move to Branch actions
    [Action("Branch exists", Description = "Branch exists in specified repository")]
    public async Task<bool> BranchExists(
        [ActionParameter] GetRepositoryRequest repositoryRequest,
        [ActionParameter] [Display("Branch name")]
        string branchNameRequest)
    {
        var branches = await Client.Repository.Branch.GetAll(long.Parse(repositoryRequest.RepositoryId));
        return branches.Any(x => x.Name == branchNameRequest);
    }
    
    [Action("File exists", Description = "Check if file exists in repository")]
    public async Task<bool> FileExists(
        [ActionParameter] GetRepositoryRequest repositoryRequest,
        [ActionParameter] GetOptionalBranchRequest branchRequest,
        [ActionParameter] GetFileRequest getFileRequest)
    {
        try
        {
            var repoInfo = await GetRepositoryById(repositoryRequest);
            var fileData = string.IsNullOrEmpty(branchRequest.Name)
                ? await Client.Repository.Content.GetRawContent(repoInfo.OwnerLogin, repoInfo.Name, getFileRequest.FilePath)
                : await Client.Repository.Content.GetRawContentByRef(repoInfo.OwnerLogin, repoInfo.Name,
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

    //[Action("Is file in folder", Description = "Is file in folder")]
    //public IsFileInFolderResponse IsFileInFolder([ActionParameter] IsFileInFolderRequest input)
    //{
    //    return new()
    //    {
    //        IsFileInFolder = input.FilePath.Split('/').SkipLast(1).Contains(input.FolderName) ? 1 : 0
    //    };
    //}

    [Action("Debug", Description = "Debug")]
    public string Debug()
    {
        return InvocationContext.AuthenticationCredentialsProviders.First(p => p.KeyName == "Authorization").Value;
    }
}