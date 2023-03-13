using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Github.Models.Responses
{
    public class UserDataResponse
    {
        public string Name { get; set; }

        public string UserUrl { get; set; }

        public int PublicRepositoriesNumber { get; set; }
    }
}
