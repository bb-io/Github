using Octokit;

namespace Apps.Github.Models.Respository.Responses;

public class RepositoryContentResponse
{
    public IEnumerable<RepositoryContent> Content { get; set; }
}