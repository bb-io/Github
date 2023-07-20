using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Github.Models.Respository.Responses
{
    public class GetRepositoryFilesFromFilepathsResponse
    {
        public IEnumerable<FileData> Files { get; set; }
    }

    public class FileData
    {
        public string Filename { get; set; }

        public byte[] File { get; set; }
    }
}
