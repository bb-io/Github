using File = Blackbird.Applications.Sdk.Common.Files.File;

namespace Apps.Github.Models.Respository.Responses
{
    public class GetRepositoryFilesFromFilepathsResponse
    {
        public IEnumerable<File> Files { get; set; }
    }
}
