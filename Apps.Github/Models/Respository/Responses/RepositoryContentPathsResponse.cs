using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Github.Models.Respository.Responses
{
    public class RepositoryContentPathsResponse
    {
        public IEnumerable<RepositoryItem> Items { get; set; }  
    }

    public class RepositoryItem
    {
        public string Sha { get; set; }

        public string Path { get; set; }
    }
}
