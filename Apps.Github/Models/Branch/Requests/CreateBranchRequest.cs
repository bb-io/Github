using Apps.GitHub.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.GitHub.Models.Branch.Requests
{
    public class CreateBranchRequest
    {
        [Display("Base branch name")]
        [DataSource(typeof(BranchDataHandler))]
        public string BaseBranchName { get; set; }

        [Display("New branch name")]
        public string NewBranchName { get; set; }
    }
}
