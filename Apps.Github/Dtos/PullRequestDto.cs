using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Github.Dtos
{
    public class PullRequestDto
    {
        public PullRequestDto(PullRequest source) {
            Title = source.Title;
            Body = source.Body;
            UserLogin = source.User.Login;
            Url = source.HtmlUrl;
        }
        public string Title { get; set; }

        public string Body { get; set; }

        public string UserLogin { get; set; }

        public string Url { get; set; }
    }
}
