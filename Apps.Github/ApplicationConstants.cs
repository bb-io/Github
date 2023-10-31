using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Github
{
    public class ApplicationConstants
    {
        public const string ClientId = "#{GITHUB_CLIENT_ID}#";

        public const string ClientSecret = "#{GITHUB_SECRET}#";

        public const string Scope = "#{GITHUB_SCOPE}#";

        public const string BlackbirdToken = "#{GITHUB_BLACKBIRD_TOKEN}#"; // bridge validates this token
    }
}