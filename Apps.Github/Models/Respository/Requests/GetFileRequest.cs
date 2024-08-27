using Apps.Github.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Github.Models.Respository.Requests;

public class GetFileRequest
{
    // Add a dynamic dropdown of tree + filter type == blob && name
    [Display("File path")]
    public string FilePath { get; set; }
}