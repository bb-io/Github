namespace Apps.Github.Models.PullRequest.Requests
{
    public class GetPullRequest
    {
        public string RepositoryId { get; set; }
        public string PullRequestNumber { get; set; }
    }
}
