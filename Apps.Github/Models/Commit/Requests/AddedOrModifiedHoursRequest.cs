using Blackbird.Applications.Sdk.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.GitHub.Models.Commit.Requests
{
    public class AddedOrModifiedHoursRequest
    {
        [Display("Last X hours", Description = "List changes in specified hours amount")]
        public int Hours { get; set; }
    }
}
