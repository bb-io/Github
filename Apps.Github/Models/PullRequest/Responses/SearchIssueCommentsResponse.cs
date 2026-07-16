using Apps.Github.Dtos;

namespace Apps.Github.Models.PullRequest.Responses;

public class SearchIssueCommentsResponse
{
    public IEnumerable<IssueCommentDto> Comments { get; set; } = [];
}
