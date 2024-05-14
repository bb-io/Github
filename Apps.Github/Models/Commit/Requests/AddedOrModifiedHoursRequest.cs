using Apps.Github.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
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

        [Display("Authors", Description = "List changes in specified hours amount")]
        [DataSource(typeof(UsersDataHandler))]
        public List<string>? Authors { get; set; }

        [Display("Exclude by authors", Description = "Exclude by authors")]
        public bool? ExcludeAuthors { get; set; }

        [Display("Exclude merge commits", Description = "Exclude merge commits")]
        public bool? ExcludeMerge { get; set; }
    }
}
