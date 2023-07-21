namespace Apps.Github.Models.Commit.Requests
{
    public class GetCommitRequest
    {
        public string RepositoryId { get; set; }

        public string CommitId { get; set; }
    }
}
