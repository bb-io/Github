using Apps.Github.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.GitHub.Models.Commit.Responses
{
    public class ListAddedOrModifiedInHoursResponse
    {
        public IEnumerable<CommitFileDto> Files { get; set; }
    }
}
