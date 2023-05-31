using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Github.Dtos
{
    public class CommitDto
    {
        public CommitDto(GitHubCommit source) 
        {
            AuthorLogin = source.Author.Login;
            Url = source.Url;
            Files = source.Files;
            Id = source.Ref;
        }

        public CommitDto(Commit source)
        {
            Id = source.Ref;
            AuthorLogin = source.Author.Name;
            Url = source.Url;
            Files = new List<GitHubCommitFile>();
        }
        public string Id { get; set; }

        public string AuthorLogin { get; set; }

        public string Url { get; set; }

        public IEnumerable<GitHubCommitFile> Files { get; set; }
    }
}
