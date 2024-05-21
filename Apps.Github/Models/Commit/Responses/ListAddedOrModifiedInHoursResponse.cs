using Apps.Github.Dtos;
using Blackbird.Applications.Sdk.Common;

namespace Apps.GitHub.Models.Commit.Responses
{
    public class ListAddedOrModifiedInHoursResponse(List<CommitFileDto> files)
    {
        public IEnumerable<CommitFileDto> Files { get; set; } = files;

        [Display("Total count")]
        public int TotalCount { get; set; } = files.Count;
    }
}
