using Octokit;

namespace Apps.Github.Dtos;

public class PullRequestCommitDto
{
    public PullRequestCommitDto(PullRequestCommit source) 
    {
        Id = source.Sha;
        Url = source.Url;
        AuthorLogin = source.Author.Login;
    }

    public string Id { get; set; }

    public string Url { get; set; }

    public string AuthorLogin { get; set; }
}