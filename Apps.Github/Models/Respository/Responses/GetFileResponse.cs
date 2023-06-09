﻿using Blackbird.Applications.Sdk.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Github.Models.Responses
{
    public class GetFileResponse
    {
        [Display("File name")]
        public string FileName { get; set; }

        [Display("Full file path")]
        public string FilePath { get; set; }

        [Display("File extension (e.g \".txt\")")]
        public string FileExtension { get; set; }

        public byte[] File { get; set; }
    }
}
