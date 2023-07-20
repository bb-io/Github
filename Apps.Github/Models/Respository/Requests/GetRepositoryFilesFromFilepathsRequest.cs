using Apps.Github.Webhooks.Payloads;
using Blackbird.Applications.Sdk.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Github.Models.Respository.Requests
{
    public class GetRepositoryFilesFromFilepathsRequest
    {
        [Display("Repository ID")]
        public string RepositoryId { get; set; }


        [Display("Filepaths array (only from webhooks)")]
        public IEnumerable<FilePathObj> Files { get; set; }
    }
}
