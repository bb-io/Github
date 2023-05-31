using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Github.Models.Requests
{
    public class RepositoryContentRequest
    {
        public string RepositoryId { get; set; }

        public string Path { get; set; }
    }
}
