using Octokit;

namespace Apps.Github.Dtos
{
    public class IssueDto
    {
        public IssueDto(Issue source) {
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
