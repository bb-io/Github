using Blackbird.Applications.Sdk.Common;

namespace Apps.Github.Models.Commit.Requests
{
    public class DeleteFileRequest
    {
        [Display("Repository ID")]
        public string RepositoryId { get; set; }

        [Display("File path")]
        public string FilePath { get; set; }

        [Display("Commit message")]
        public string CommitMessage { get; set; }
    }
}
