using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Github.Models.Requests
{
    public class GetFileRequest
    {
        public string RepositoryId { get; set; }
        public string FilePath { get; set; }
    }
}
