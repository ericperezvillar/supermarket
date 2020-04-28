using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Helpers
{
    public class AppSettings
    {
        public string Secret { get; set; }

        public int ExpiryTimeInSeconds { get; set; }

        public string Issuer { get; set; }

        public string Audience { get; set; }
    }
}
