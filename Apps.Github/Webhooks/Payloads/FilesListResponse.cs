using Blackbird.Applications.Sdk.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Github.Webhooks.Payloads
{
    public class FilesListResponse
    {
        public IEnumerable<FileId> Files { get; set; }
    }

    public class FileId
    {

        [Display("File path")]
        public string FilePath { get; set; }
    }
}
