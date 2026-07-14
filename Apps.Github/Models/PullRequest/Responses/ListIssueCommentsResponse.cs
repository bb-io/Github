using Apps.Github.Dtos;

namespace Apps.Github.Models.PullRequest.Responses;

public class ListIssueCommentsResponse
{
    public IEnumerable<IssueCommentDto> Comments { get; set; } = [];
}
