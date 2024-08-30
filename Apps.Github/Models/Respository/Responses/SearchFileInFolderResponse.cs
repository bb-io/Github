using Apps.Github.Webhooks;
using Apps.GitHub.Dtos;
using Octokit;

namespace Apps.GitHub.Models.Respository.Responses
{
    public class SearchFileInFolderResponse
    {
        public SearchFileInFolderResponse(IEnumerable<RepositoryContent> repositoryContent, string? wildcard)
        {
            if (!string.IsNullOrEmpty(wildcard))
            {
                repositoryContent = repositoryContent.Where(x => PushWebhooks.IsFilePathMatchingPattern(wildcard, x.Path));
            }
            Files = repositoryContent.Where(x => x.Type.Value == ContentType.File).Select(x => new FileMetadataDto(x)).ToList();
        }

        public List<FileMetadataDto> Files { get; set; }
    }
}
