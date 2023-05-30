using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Github.Models.Requests
{
    public class GetFileRequest
    {
        public string RepositoryOwnerLogin { get; set; }
        public string RepositoryName { get; set; }
        public string FilePath { get; set; }
    }
}
