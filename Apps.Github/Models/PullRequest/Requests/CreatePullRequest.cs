namespace Apps.Github.Models.PullRequest.Requests
{
    public class CreatePullRequest
    {
        public string RepositoryId { get; set; }

        public string Title { get; set; }

        public string HeadBranch { get; set; }

        public string BaseBranch { get; set; }
    }
}
