using Apps.Github.Dtos;

namespace Apps.Github.Models.Responses
{
    public class GetPullRequestsResponse
    {
        public IEnumerable<PullRequestDto> PullRequests { get; set; }
    }
}
