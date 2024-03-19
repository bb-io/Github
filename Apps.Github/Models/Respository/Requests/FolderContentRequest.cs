using Apps.Github.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Github.Models.Respository.Requests;

public class FolderContentRequest
{
    [Display("Folder path (e.g. \"Folder1/Folder2\")")]
    public string? Path { get; set; }
}