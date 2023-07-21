using Octokit;

namespace Apps.Github.Dtos
{
    public class MergeDto
    {
        public MergeDto(Merge source) {
            Id = source.Ref;
        }

        public string Id { get; set; }
    }
}
