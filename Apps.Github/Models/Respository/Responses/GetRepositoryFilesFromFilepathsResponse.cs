using Apps.Github.Models.Commit.Responses;

namespace Apps.Github.Models.Respository.Responses;

public class GetRepositoryFilesFromFilepathsResponse
{
    public IEnumerable<GithubFile> Files { get; set; }
}