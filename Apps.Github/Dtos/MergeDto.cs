using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Github.Dtos
{
    public class MergeDto
    {
        public MergeDto(Merge source) {
            Id = source.Ref;
        }

        public string Id { get; set; }
    }
}
