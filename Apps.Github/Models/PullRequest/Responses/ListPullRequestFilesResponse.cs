using Octokit;

namespace Apps.Github.Models.PullRequest.Responses
{
    public class ListPullRequestFilesResponse
    {
        public IEnumerable<PullRequestFile> Files { get; set; }
    }
}
