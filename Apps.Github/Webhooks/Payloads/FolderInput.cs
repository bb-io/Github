using Blackbird.Applications.Sdk.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Github.Webhooks.Payloads
{
    public class FolderInput
    {
        [Display("Folder path")]
        public string? FolderPath { get; set; }
    }
}
