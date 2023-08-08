using Apps.Github.Dtos;
using Apps.Github.Models.Commit.Requests;
using Apps.Github.Models.Commit.Responses;
using Apps.Github.Models.Respository.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;

namespace Apps.Github.Actions
{
    [ActionList]
    public class CommitActions
    {
        [Action("List commits", Description = "List respository commits")]
        public ListRepositoryCommitsResponse ListRepositoryCommits(
            IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] ListRepositoryCommitsRequest input)
        {
            var client = new BlackbirdGithubClient(authenticationCredentialsProviders);
            var commits = client.Repository.Commit.GetAll(long.Parse(input.RepositoryId)).Result;
            return new ListRepositoryCommitsResponse
            {
                Commits = commits.Select(c => new SmallCommitDto(c))
            };
        }

        [Action("Get commit", Description = "Get commit by id")]
        public FullCommitDto GetCommit(
            IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] GetCommitRequest input)
        {
            if (!long.TryParse(input.RepositoryId, out var intRepoId))
                throw new("Wrong repository ID");

            var client = new BlackbirdGithubClient(authenticationCredentialsProviders);
            var commit = client.Repository.Commit.Get(intRepoId, input.CommitId).Result;
            return new(commit);
        }

        [Action("Push file", Description = "Push file to repository")]
        public SmallCommitDto PushFile(
            IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] PushFileRequest input)
        {
            var client = new BlackbirdGithubClient(authenticationCredentialsProviders);

            var repContent = new RepositoryActions().ListAllRepositoryContent(authenticationCredentialsProviders,
                new ListAllRepositoryContentRequest
                {
                    RepositoryId = input.RepositoryId,
                });
            if (repContent.Items.Any(p => p.Path == input.DestinationFilePath)) // update in case of existing file
            {
                return UpdateFile(authenticationCredentialsProviders, new UpdateFileRequest
                {
                    FileId = repContent.Items.First(p => p.Path == input.DestinationFilePath).Sha,
                    DestinationFilePath = input.DestinationFilePath,
                    File = input.File,
                    RepositoryId = input.RepositoryId,
                    CommitMessage = input.CommitMessage
                });
            }

            var fileUpload =
                new Octokit.CreateFileRequest(input.CommitMessage, Convert.ToBase64String(input.File), false);
            var pushFileResult = client.Repository.Content
                .CreateFile(long.Parse(input.RepositoryId), input.DestinationFilePath, fileUpload).Result;
            return new(pushFileResult.Commit);
        }

        [Action("Update file", Description = "Update file in repository")]
        public SmallCommitDto UpdateFile(
            IEnumerable<AuthenticationCredentialsProvider> creds,
            [ActionParameter] UpdateFileRequest input)
        {
            var client = new BlackbirdGithubClient(creds);

            var fileId = input.FileId ?? GetFileId(creds, input.RepositoryId, input.DestinationFilePath);

            var fileUpload = new Octokit.UpdateFileRequest(input.CommitMessage, Convert.ToBase64String(input.File),
                fileId, false);
            var pushFileResult = client.Repository.Content
                .UpdateFile(long.Parse(input.RepositoryId), input.DestinationFilePath, fileUpload).Result;

            return new(pushFileResult.Commit);
        }

        [Action("Delete file", Description = "Delete file from repository")]
        public Task DeleteFile(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] DeleteFileRequest input)
        {
            var client = new BlackbirdGithubClient(authenticationCredentialsProviders);

            var fileId = GetFileId(authenticationCredentialsProviders, input.RepositoryId, input.FilePath);

            var fileDelete = new Octokit.DeleteFileRequest(input.CommitMessage, fileId);
            return client.Repository.Content.DeleteFile(long.Parse(input.RepositoryId), input.FilePath, fileDelete);
        }

        private string GetFileId(IEnumerable<AuthenticationCredentialsProvider> creds, string repoId, string path)
        {
            var repoContent = new RepositoryActions().ListAllRepositoryContent(creds, new()
            {
                RepositoryId = repoId
            });

            return repoContent.Items.First(x => x.Path == path).Sha;
        }
    }
}