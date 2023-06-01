using Blackbird.Applications.Sdk.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Github.Models.PullRequest.Responses
{
    public class IsPullMergedResponse
    {
        [Display("Is merged")]
        public bool IsPullMerged { get; set; }
    }
}
