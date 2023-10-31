using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Github
{
    public class ApplicationConstants
    {
        public const string ClientId = "#{APP_GITHUB_CLIENT_ID}#";

        public const string ClientSecret = "#{APP_GITHUB_SECRET}#";

        public const string Scope = "#{APP_GITHUB_SCOPE}#";

        public const string BlackbirdToken = "#{APP_GITHUB_BLACKBIRD_TOKEN}#"; // bridge validates this token
    }
}