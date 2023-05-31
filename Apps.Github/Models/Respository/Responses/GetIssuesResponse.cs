using Apps.Github.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Github.Models.Respository.Responses
{
    public class GetIssuesResponse
    {
        public IEnumerable<IssueDto> Issues { get; set; }
    }
}
