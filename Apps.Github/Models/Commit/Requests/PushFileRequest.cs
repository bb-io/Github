using Blackbird.Applications.Sdk.Common;

namespace Apps.Github.Models.Commit.Requests
{
    public class PushFileRequest
    {
        [Display("Repository ID")]
        public string RepositoryId { get; set; }

        [Display("Destination file path (e.g. \"Test/testFile.txt\")")]
        public string DestinationFilePath { get; set; }

        public byte[] File { get; set; }

        [Display("Commit message")]
        public string CommitMessage { get; set; }
    }
}
