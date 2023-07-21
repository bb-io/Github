using Octokit;

namespace Apps.Github.Dtos
{
    public class PullRequestDto
    {
        public PullRequestDto(PullRequest source) {
            Id = source.Id.ToString();
            Title = source.Title;
            Body = source.Body;
            UserLogin = source.User.Login;
            Url = source.HtmlUrl;
            Number = source.Number.ToString();
        }
        public string Id { get; set; }

        public string Number { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public string UserLogin { get; set; }

        public string Url { get; set; }
    }
}
