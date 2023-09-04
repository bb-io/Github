using Apps.Github.Dtos;

namespace Apps.Github.Models.PullRequest.Responses;

public class ListPullRequestCommitsResponse
{
    public IEnumerable<PullRequestCommitDto> Commits { get; set; }
}