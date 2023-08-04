namespace Apps.Github.Models.Commit.Requests
{
    public class DeleteFileRequest
    {
        public string RepositoryId { get; set; }

        public string FilePath { get; set; }

        public string CommitMessage { get; set; }
    }
}
