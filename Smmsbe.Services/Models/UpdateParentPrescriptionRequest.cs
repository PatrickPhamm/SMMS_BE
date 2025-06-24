using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smmsbe.Services.Models
{
    public class UpdateParentPrescriptionRequest
    {
        public int PrescriptionId { get; set; }
        public string Schedule { get; set; }
        public string ParentNote { get; set; }
        public string PrescriptionFile { get; set; }
    }
}
