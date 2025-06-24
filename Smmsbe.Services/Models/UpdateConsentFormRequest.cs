using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smmsbe.Services.Models
{
    public class UpdateConsentFormRequest
    {
        public int ConsentFormId { get; set; }
        public int? FormId { get; set; }
        public int? ParentId { get; set; }
    }
}
