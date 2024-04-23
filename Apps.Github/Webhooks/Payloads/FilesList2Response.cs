using Blackbird.Applications.Sdk.Common;
using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.GitHub.Webhooks.Payloads
{
    public class FilesList2Response
    {
        [Display("File paths")]
        public IEnumerable<GitHubCommit> Files { get; set; }
        //public IEnumerable<FilePathObj> Files { get; set; }
    }
}
