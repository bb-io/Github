using Blackbird.Applications.Sdk.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.GitHub.Webhooks.Payloads
{
    public class BranchInput
    {
        [Display("Branch name")]
        public string? BranchName { get; set; }
    }
}
