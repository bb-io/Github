﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Github.Models.Requests
{
    public class RepositoryRequest
    {
        public string RepositoryOwnerLogin { get; set; }

        public string RepositoryName { get; set; }
    }
}
