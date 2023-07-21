namespace Apps.Github.Models.Branch.Requests
{
    public class MergeBranchRequest
    {
        public string RepositoryId { get; set; }
        public string BaseBranch { get; set; }
        public string HeadBranch { get; set; }

        public string CommitMessage { get; set; }
    }
}
