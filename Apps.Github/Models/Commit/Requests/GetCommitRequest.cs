using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Github.Models.Commit.Requests
{
    public class GetCommitRequest
    {
        public string RepositoryId { get; set; }

        public string CommitId { get; set; }
    }
}
