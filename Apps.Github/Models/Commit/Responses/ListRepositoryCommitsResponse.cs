using Apps.Github.Dtos;
using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Github.Models.Commit.Responses
{
    public class ListRepositoryCommitsResponse
    {
        public IEnumerable<CommitDto> Commits { get; set; }
    }
}
