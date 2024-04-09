using Apps.Github.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.Github.Models.Commit.Requests;

public class PushFileRequest
{
    [Display("Destination file path (e.g. \"Test/testFile.txt\")")]
    public string DestinationFilePath { get; set; }

    [Display("File")]
    public FileReference File { get; set; }

    [Display("Commit message")]
    public string CommitMessage { get; set; }
}