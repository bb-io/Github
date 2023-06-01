using Apps.Github.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Github.Models.PullRequest.Responses
{
    public class ListPullRequestCommitsResponse
    {
        public IEnumerable<PullRequestCommitDto> Commits { get; set; }
    }
}
