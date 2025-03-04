using Octokit;

namespace Apps.Github.Dtos;

public class PullRequestCommitDto(PullRequestCommit source)
{
    public string Id { get; set; } = source.Sha;

    public string Url { get; set; } = source.Url;

    public string AuthorLogin { get; set; } = source.Author.Login;
}