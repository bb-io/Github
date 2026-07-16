using Apps.GitHub.Dtos;
using Blackbird.Applications.Sdk.Common;

namespace Apps.Github.Models.PullRequest.Responses;

public class SearchPullRequestFilesResponse
{
    public IEnumerable<PullRequestFileDto> Files { get; set; } = [];

    [Display("Files JSON")]
    public string FilesJson { get; set; } = "[]";
}
