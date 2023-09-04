using Apps.Github.Dtos;
using Blackbird.Applications.Sdk.Common;

namespace Apps.Github.Models.Respository.Responses;

public class GetPullRequestsResponse
{
    [Display("Pull requests")]
    public IEnumerable<PullRequestDto> PullRequests { get; set; }
}