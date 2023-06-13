using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Github.Webhooks.Payloads
{
    public class FilesListResponse
    {
        public string FirstFilename { get; set; }
        public IEnumerable<FileId> AllFiles { get; set; }
    }

    public class FileId
    {
        public string Id { get; set; }
    }
}
