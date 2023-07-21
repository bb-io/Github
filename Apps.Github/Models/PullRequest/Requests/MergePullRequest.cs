namespace Apps.Github.Models.PullRequest.Requests
{
    public class MergePullRequest
    {
        public string RepositoryId { get; set; }

        public int PullRequestNumber { get; set; }
    }
}
