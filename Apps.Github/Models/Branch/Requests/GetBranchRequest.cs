using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Github.Models.Branch.Requests
{
    public class GetBranchRequest
    {
        public string RepositoryId { get; set; }
        public string Name { get; set; }
    }
}
