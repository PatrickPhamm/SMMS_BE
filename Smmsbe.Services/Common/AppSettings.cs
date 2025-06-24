using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smmsbe.Services.Common
{
    public class AppSettings
    {
        public string ApplicationUrl { get; set; }
        public string LandingPageUrl { get; set; }
        public EmailSettings EmailSettings { get; set; }
    }
}
