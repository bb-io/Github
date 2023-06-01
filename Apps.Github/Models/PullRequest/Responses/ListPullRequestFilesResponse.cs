using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Github.Models.PullRequest.Responses
{
    public class ListPullRequestFilesResponse
    {
        public IEnumerable<PullRequestFile> Files { get; set; }
    }
}
