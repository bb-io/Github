using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Github.Models.Respository.Requests
{
    public class IsFileInFolderRequest
    {
        public string FilePath { get; set; }

        public string FolderName { get; set; }
    }
}
