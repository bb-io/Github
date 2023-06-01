using Apps.Github.Dtos;
using Apps.Github.Models.Commit.Requests;
using Apps.Github.Models.Commit.Responses;
using Apps.Github.Models.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;
using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Github.Actions
{
    [ActionList]
    public class CommitActions
    {

        [Action("List commits", Description = "List respository commits")]
        public ListRepositoryCommitsResponse ListRepositoryCommits(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] ListRepositoryCommitsRequest input)
        {
            var client = new BlackbirdGithubClient(authenticationCredentialsProviders);
            var commits = client.Repository.Commit.GetAll(long.Parse(input.RepositoryId)).Result;
            return new ListRepositoryCommitsResponse()
            {
                Commits = commits.Select(c => new CommitDto(c))
            };
        }

        [Action("Get commit", Description = "Get commit by id")]
        public CommitDto GetCommit(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] GetCommitRequest input)
        {
            var client = new BlackbirdGithubClient(authenticationCredentialsProviders);
            var commit = client.Repository.Commit.Get(long.Parse(input.RepositoryId), input.CommitId).Result;
            return new CommitDto(commit);
        }

        [Action("Push file", Description = "Push file to repository")]
        public CommitDto PushFile(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] PushFileRequest input)
        {
            var client = new BlackbirdGithubClient(authenticationCredentialsProviders);

            var fileUpload = new Octokit.CreateFileRequest(input.CommitMessage, Convert.ToBase64String(input.File), false);
            var pushFileResult = client.Repository.Content.CreateFile(long.Parse(input.RepositoryId), input.DestinationFilePath, fileUpload).Result;
            return new CommitDto(pushFileResult.Commit);
        }

        [Action("Update file", Description = "Update file in repository")]
        public CommitDto UpdateFile(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] Models.Commit.Requests.UpdateFileRequest input)
        {
            var client = new BlackbirdGithubClient(authenticationCredentialsProviders);

            var fileUpload = new Octokit.UpdateFileRequest(input.CommitMessage, Convert.ToBase64String(input.File), input.FileId, false);
            var pushFileResult = client.Repository.Content.UpdateFile(long.Parse(input.RepositoryId), input.DestinationFilePath, fileUpload).Result;
            return new CommitDto(pushFileResult.Commit);
        }

        [Action("Delete file", Description = "Delete file from repository")]
        public void DeleteFile(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] Models.Commit.Requests.DeleteFileRequest input)
        {
            var client = new BlackbirdGithubClient(authenticationCredentialsProviders);

            var fileDelete = new Octokit.DeleteFileRequest(input.CommitMessage, input.FileId);
            var deleteFileResult = client.Repository.Content.DeleteFile(long.Parse(input.RepositoryId), input.FilePath, fileDelete);
        }
    }
}
