using Octokit;

namespace Apps.Github.Models.Responses
{
    public class RepositoryContentResponse
    {
        public IEnumerable<RepositoryContent> Content { get; set; }
    }
}
