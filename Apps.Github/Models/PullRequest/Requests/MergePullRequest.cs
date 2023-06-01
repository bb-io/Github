using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Github.Models.PullRequest.Requests
{
    public class MergePullRequest
    {
        public string RepositoryId { get; set; }

        public int PullRequestNumber { get; set; }
    }
}
