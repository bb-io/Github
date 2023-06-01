using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Github.Models.Branch.Requests
{
    public class MergeBranchRequest
    {
        public string RepositoryId { get; set; }
        public string BaseBranch { get; set; }
        public string HeadBranch { get; set; }

        public string CommitMessage { get; set; }
    }
}
