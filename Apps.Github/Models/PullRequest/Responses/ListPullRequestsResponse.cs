using Apps.Github.Dtos;
using Blackbird.Applications.Sdk.Common;

namespace Apps.Github.Models.PullRequest.Responses;

public class ListPullRequestsResponse
{
    [Display("Pull requests")]
    public IEnumerable<PullRequestDto> PullRequests { get; set; }
}