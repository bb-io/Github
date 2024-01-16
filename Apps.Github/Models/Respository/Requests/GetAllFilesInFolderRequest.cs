using Blackbird.Applications.Sdk.Common;
using Apps.Github.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Github.Models.Respository.Requests;

public class GetAllFilesInFolderRequest
{
    [Display("Folder path (e.g. \"Folder1/Folder2\")")]
    public string FolderPath { get; set; }

    [Display("Repository ID")]
    [DataSource(typeof(RepositoryDataHandler))]
    public string RepositoryId { get; set; }
}