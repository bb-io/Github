using Apps.GitHub.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.SDK.Extensions.FileManagement.Models.FileDataSourceItems;

namespace Apps.Github.Models.Commit.Requests;

public class CreateOrUpdateFileRequest
{
    [Display("Folder path", Description = "The folder this file is uploaded to")]
    [FileDataSource(typeof(FolderPathDataHandler))]
    public string? FolderPath { get; set; }

    [Display("File name", Description = "File name, either with or without extension")]
    public string? NewFileName { get; set; }

    [Display("File")]
    public FileReference File { get; set; }

    [Display("Commit message")]
    public string CommitMessage { get; set; }
}