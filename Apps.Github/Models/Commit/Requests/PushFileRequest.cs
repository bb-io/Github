using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Github.Models.Commit.Requests
{
    public class PushFileRequest
    {
        public string RepositoryId { get; set; }

        public string DestinationFilePath { get; set; }

        public byte[] File { get; set; }

        public string CommitMessage { get; set; }
    }
}
