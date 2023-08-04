using Blackbird.Applications.Sdk.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Github.Models.Respository.Requests
{
    public class GetAllFilesInFolderRequest
    {
        [Display("Folder path (e.g. \"Folder1/Folder2\")")]
        public string FolderPath { get; set; }

        [Display("Repository ID")]
        public string RepositoryId { get; set; }
    }
}
