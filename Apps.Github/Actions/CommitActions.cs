using Apps.Github.Actions.Base;
using Apps.Github.Dtos;
using Apps.Github.Models.Commit.Requests;
using Apps.Github.Models.Commit.Responses;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Github.Actions;

[ActionList]
public class CommitActions : GithubActions
{
    public CommitActions(InvocationContext invocationContext) : base(invocationContext)
    {
    }

    [Action("List commits", Description = "List respository commits")]
    public ListRepositoryCommitsResponse ListRepositoryCommits(
        [ActionParameter] ListRepositoryCommitsRequest input)
    {
        var commits = Client.Repository.Commit.GetAll(long.Parse(input.RepositoryId)).Result;

        return new()
        {
            Commits = commits.Select(c => new SmallCommitDto(c))
        };
    }

    [Action("Get commit", Description = "Get commit by id")]
    public FullCommitDto GetCommit([ActionParameter] GetCommitRequest input)
    {
        if (!long.TryParse(input.RepositoryId, out var intRepoId))
            throw new("Wrong repository ID");

        var commit = Client.Repository.Commit.Get(intRepoId, input.CommitId).Result;
        return new(commit);
    }

    [Action("Push file", Description = "Push file to repository")]
    public SmallCommitDto PushFile([ActionParameter] PushFileRequest input)
    {
        var repContent = new RepositoryActions(InvocationContext).ListAllRepositoryContent(
            new()
            {
                RepositoryId = input.RepositoryId,
            });
        if (repContent.Items.Any(p => p.Path == input.DestinationFilePath)) // update in case of existing file
        {
            return UpdateFile(new()
            {
                FileId = repContent.Items.First(p => p.Path == input.DestinationFilePath).Sha,
                DestinationFilePath = input.DestinationFilePath,
                File = input.File,
                RepositoryId = input.RepositoryId,
                CommitMessage = input.CommitMessage
            });
        }

        var fileUpload =
            new Octokit.CreateFileRequest(input.CommitMessage, Convert.ToBase64String(input.File.Bytes), false);
        var pushFileResult = Client.Repository.Content
            .CreateFile(long.Parse(input.RepositoryId), input.DestinationFilePath, fileUpload).Result;
        return new(pushFileResult.Commit);
    }

    [Action("Update file", Description = "Update file in repository")]
    public SmallCommitDto UpdateFile([ActionParameter] UpdateFileRequest input)
    {
        var fileId = input.FileId ?? GetFileId(input.RepositoryId, input.DestinationFilePath);

        var fileUpload = new Octokit.UpdateFileRequest(input.CommitMessage, Convert.ToBase64String(input.File.Bytes),
            fileId, false);
        fileUpload.Branch = input.BranchName;
        var pushFileResult = Client.Repository.Content
            .UpdateFile(long.Parse(input.RepositoryId), input.DestinationFilePath, fileUpload).Result;

        return new(pushFileResult.Commit);
    }

    [Action("Delete file", Description = "Delete file from repository")]
    public Task DeleteFile([ActionParameter] DeleteFileRequest input)
    {
        var fileId = GetFileId(input.RepositoryId, input.FilePath);

        var fileDelete = new Octokit.DeleteFileRequest(input.CommitMessage, fileId);
        return Client.Repository.Content.DeleteFile(long.Parse(input.RepositoryId), input.FilePath, fileDelete);
    }

    private string GetFileId(string repoId, string path)
    {
        var repoContent = new RepositoryActions(InvocationContext).ListAllRepositoryContent(new()
        {
            RepositoryId = repoId
        });

        return repoContent.Items.First(x => x.Path == path).Sha;
    }
}