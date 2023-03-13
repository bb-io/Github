using Apps.Github.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Github.Models.Responses
{
    public class GetPullRequestsResponse
    {
        public IEnumerable<PullRequestDto> PullRequests { get; set; }
    }
}
