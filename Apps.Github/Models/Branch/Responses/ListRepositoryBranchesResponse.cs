using Apps.Github.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Github.Models.Branch.Responses
{
    public class ListRepositoryBranchesResponse
    {
        public IEnumerable<BranchDto> Branches { get; set; }
    }
}
