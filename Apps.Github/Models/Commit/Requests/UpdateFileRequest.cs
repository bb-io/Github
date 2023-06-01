using Apps.Github.Models.Commit.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Github.Models.Commit.Requests
{
    public class UpdateFileRequest : PushFileRequest
    {
        public string FileId { get; set; }
    }
}
