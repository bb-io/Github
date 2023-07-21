using Apps.Github.Dtos;

namespace Apps.Github.Models.Commit.Responses
{
    public class ListRepositoryCommitsResponse
    {
        public IEnumerable<CommitDto> Commits { get; set; }
    }
}
