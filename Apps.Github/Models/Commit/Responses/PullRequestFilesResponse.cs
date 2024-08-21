using Apps.Github.Dtos;
using Apps.GitHub.Dtos;
using Blackbird.Applications.Sdk.Common;

namespace Apps.GitHub.Models.Commit.Responses
{
    public class PullRequestFilesResponse(List<PullRequestFileDto> files)
    {
        public IEnumerable<PullRequestFileDto> Files { get; set; } = files;

        [Display("Total count")]
        public int TotalCount { get; set; } = files.Count;
    }
}
