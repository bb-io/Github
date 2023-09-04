using Apps.Github.Dtos;

namespace Apps.Github.Models.Commit.Responses;

public class ListRepositoryCommitsResponse
{
    public IEnumerable<SmallCommitDto> Commits { get; set; }
}