using Apps.Github.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Github.Models.Responses
{
    public class RepositoryContentResponse
    {
        public IEnumerable<string> Content { get; set; }
    }
}
