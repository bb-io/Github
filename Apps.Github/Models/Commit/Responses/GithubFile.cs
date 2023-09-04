using Blackbird.Applications.Sdk.Common;
using File = Blackbird.Applications.Sdk.Common.Files.File;

namespace Apps.Github.Models.Commit.Responses;

public class GithubFile
{
    [Display("File path")]
    public string FilePath { get; set; }
    
    public File File { get; set; }
}