using Apps.Github.Webhooks;
using Apps.GitHub.Dtos;
using Blackbird.Applications.Sdk.Common;
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

        public SearchFileInFolderResponse(TreeResponse repositoryContent, string? path, string? wildcard)
        {
            var filteredFiles = !string.IsNullOrEmpty(wildcard) ? repositoryContent.Tree.Where(x => PushWebhooks.IsFilePathMatchingPattern(wildcard, x.Path)) : repositoryContent.Tree;
            Files = filteredFiles.Where(x => x.Type == TreeType.Blob).Select(x => new FileMetadataDto(x)).ToList();

            if (path != null)
            {
                foreach (var file in Files)
                {
                    file.Path = path + "/" + file.Path;
                }
            }
        }

        public List<FileMetadataDto> Files { get; set; }

        [Display("Truncated", Description = "Whether the result from Github was completed. If false, the repository is too big to be searched by Blackbird.")]
        public bool Truncated { get; set; }
    }
}
