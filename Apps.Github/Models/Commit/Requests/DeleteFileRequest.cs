using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Github.Models.Commit.Requests
{
    public class DeleteFileRequest
    {
        public string RepositoryId { get; set; }

        public string FilePath { get; set; }

        public string FileId { get; set; }

        public string CommitMessage { get; set; }
    }
}
