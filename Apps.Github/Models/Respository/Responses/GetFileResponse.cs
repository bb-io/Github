using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Github.Models.Responses
{
    public class GetFileResponse
    {
        public string FileName { get; set; }

        public byte[] File { get; set; }
    }
}
