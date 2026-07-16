using Apps.Github.Dtos;

namespace Apps.Github.Models.PullRequest.Responses;

public class SearchPullRequestReviewCommentsResponse
{
    public IEnumerable<PullRequestReviewCommentDto> Comments { get; set; } = [];
}
