using Apps.GitHub.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.SDK.Extensions.FileManagement.Models.FileDataSourceItems;

namespace Apps.Github.Webhooks.Payloads;

public class FolderInput
{
    [Display("Folder path", Description = "Use the forward slash '/' to represent directory separator. Use '*' to represent wildcards in file and directory names. Use '**' to represent arbitrary directory depth.")]
    public string? FolderPath { get; set; }

    [FileDataSource(typeof(FolderPathDataHandler))]
    public string? Folder { get; set; }

    [Display("Recursively include subfolders")]
    public bool? RecursivelyIncludeSubfolders { get; set; }
}