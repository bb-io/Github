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
        public IEnumerable<TempOutputClass> Files { get; set; }
        //public IEnumerable<FilePathObj> Files { get; set; }
    }

    public class TempOutputClass
    {
        public TempOutputClass(GitHubCommit gitCom)
        {
            HtmlUrl = gitCom.HtmlUrl;
            if(gitCom.Files != null)
            {
                Files = gitCom.Files.Select(x => new GithubComFile()
                {
                    Filename = x.Filename,
                    Additions = x.Additions,
                    Deletions = x.Deletions,
                    Changes = x.Changes,
                    Status = x.Status,
                    Sha = x.Sha
                }).ToList();
            }
            
        }

        public string HtmlUrl { get; set; }

        public List<GithubComFile> Files { get; set; }
    }

    public class GithubComFile()
    {
        public string Filename { get; set; }

        /// <summary>
        /// Number of additions performed on the file.
        /// </summary>
        public int Additions { get; set; }

        /// <summary>
        /// Number of deletions performed on the file.
        /// </summary>
        public int Deletions { get; set; }

        /// <summary>
        /// Number of changes performed on the file.
        /// </summary>
        public int Changes { get; set; }

        /// <summary>
        /// File status, like modified, added, deleted.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// The url to the file blob.
        /// </summary>
        public string BlobUrl { get; set; }

        /// <summary>
        /// The url to file contents API.
        /// </summary>
        public string ContentsUrl { get; set; }

        /// <summary>
        /// The raw url to download the file.
        /// </summary>
        public string RawUrl { get; set; }

        /// <summary>
        /// The SHA of the file.
        /// </summary>
        public string Sha { get; set; }

        /// <summary>
        /// The patch associated with the commit
        /// </summary>
        public string Patch { get; set; }
    }
}
