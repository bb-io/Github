using Blackbird.Applications.Sdk.Common;

namespace Apps.Github.Models.Respository.Responses
{
    public class GetRepositoryFilesFromFilepathsResponse
    {
        public IEnumerable<FileData> Files { get; set; }
    }

    public class FileData
    {
        public string Filename { get; set; }

        public byte[] File { get; set; }
    }
}
