using Apps.Github.Dtos;

namespace Apps.Github.Models.PullRequest.Responses
{
    public class ListPullRequestsResponse
    {
        public IEnumerable<PullRequestDto> PullRequests { get; set; }
    }
}
