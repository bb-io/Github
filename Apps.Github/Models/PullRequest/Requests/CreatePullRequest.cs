using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Github.Models.PullRequest.Requests
{
    public class CreatePullRequest
    {
        public string RepositoryId { get; set; }

        public string Title { get; set; }

        public string HeadBranch { get; set; }

        public string BaseBranch { get; set; }
    }
}
