using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smmsbe.Services.Models
{
    public class AddParentPrescriptionRequest
    {
        public int? NurseId { get; set; }
        public DateOnly? SubmittedDate { get; set; }
        public string Schedule { get; set; }
        public string ParentNote { get; set; }
        public int? ParentId { get; set; }
        public string PrescriptionFile { get; set; }
    }
}
