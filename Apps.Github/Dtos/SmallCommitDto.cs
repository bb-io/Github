using Blackbird.Applications.Sdk.Common;
using Octokit;

namespace Apps.Github.Dtos;

public class SmallCommitDto
{
    public string Id { get; set; }
    [Display("Author login")] public string AuthorLogin { get; set; }
    public string Url { get; set; }
    public string Message { get; set; }
    
    public SmallCommitDto(GitHubCommit source)
    {
        Id = source.Sha;
        AuthorLogin = source.Author.Login;
        Message = source.Commit.Message;
        Url = source.Url;
    }
    
    public SmallCommitDto(Commit source)
    {
        Id = source.Ref;
        AuthorLogin = source.Author.Name;
        Message = source.Message;
        Url = source.Url;
    }
}