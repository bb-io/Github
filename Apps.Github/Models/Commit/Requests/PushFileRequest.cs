namespace Apps.Github.Models.Commit.Requests
{
    public class PushFileRequest
    {
        public string RepositoryId { get; set; }

        public string DestinationFilePath { get; set; }

        public byte[] File { get; set; }

        public string CommitMessage { get; set; }
    }
}
