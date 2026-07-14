using Apps.GitHub.Dtos;

namespace Apps.Github.Models.PullRequest.Responses;

public class ListPullRequestFilesResponse
{
    public IEnumerable<PullRequestFileDto> Files { get; set; } = [];
}
