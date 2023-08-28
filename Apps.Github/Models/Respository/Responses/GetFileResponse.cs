using Blackbird.Applications.Sdk.Common;
using File = Blackbird.Applications.Sdk.Common.Files.File;

namespace Apps.Github.Models.Respository.Responses
{
    public class GetFileResponse
    {
        [Display("Full file path")]
        public string FilePath { get; set; }

        [Display("File")]
        public File File { get; set; }

        [Display("File extension (e.g \".txt\")")]
        public string FileExtension { get; set; }
    }
}
