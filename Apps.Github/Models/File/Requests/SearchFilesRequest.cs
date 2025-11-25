using Apps.GitHub.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.SDK.Extensions.FileManagement.Models.FileDataSourceItems;

namespace Apps.GitHub.Models.File.Requests;

public class SearchFilesRequest
{
    public SearchFilesRequest()
    {
    }

    public SearchFilesRequest(string? path, bool? includeSubfolders)
    {
        Path = path;
        IncludeSubfolders = includeSubfolders;
    }

    [Display("Folder", Description = "e.g. \"Folder1/Folder2\". The default path is the root folder.")]
    [FileDataSource(typeof(FolderPathDataHandler))]
    public string? Path { get; set; }

    [Display("Recursively include subfolders", Description = "If set, will also include all files in all subfolders to your path.")]
    public bool? IncludeSubfolders { get; set; }

    [Display("Filter", Description = "Use the forward slash '/' to represent directory separator. Use '*' to represent wildcards in file and directory names. Use '**' to represent arbitrary directory depth.")]
    public string? Filter { get; set; }
}