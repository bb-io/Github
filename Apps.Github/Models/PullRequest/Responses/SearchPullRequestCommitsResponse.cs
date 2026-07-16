using Apps.Github.Dtos;

namespace Apps.Github.Models.PullRequest.Responses;

public class SearchPullRequestCommitsResponse
{
    public IEnumerable<PullRequestCommitDto> Commits { get; set; } = [];
}
