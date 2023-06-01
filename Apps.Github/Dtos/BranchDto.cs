using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Github.Dtos
{
    public class BranchDto
    {
        public BranchDto(Branch source) 
        {
            Name = source.Name;
            Protected = source.Protected;
        }

        public string Name { get; set; }

        public bool Protected { get; set; }
    }
}
