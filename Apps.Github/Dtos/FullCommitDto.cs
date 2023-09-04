using Octokit;

namespace Apps.Github.Dtos;

public class FullCommitDto : SmallCommitDto
{
    public IEnumerable<CommitFileDto> Files { get; set; }

    public FullCommitDto(GitHubCommit source) : base(source)
    {
        Files = source.Files.Select(x => new CommitFileDto(x));
    }
}