namespace Apps.Github.Models.PullRequest.Requests
{
    public class ListPullFilesRequest
    {
        public string RepositoryId { get; set; }

        public string PullRequestNumber { get; set; }
    }
}
