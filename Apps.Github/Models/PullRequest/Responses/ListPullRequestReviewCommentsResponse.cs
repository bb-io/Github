using Apps.Github.Dtos;

namespace Apps.Github.Models.PullRequest.Responses;

public class ListPullRequestReviewCommentsResponse
{
    public IEnumerable<PullRequestReviewCommentDto> Comments { get; set; } = [];
}
