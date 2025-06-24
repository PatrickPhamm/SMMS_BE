using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smmsbe.Services.Models
{
    public class AddMedicationRequest
    {
        public int? StudentId { get; set; }
        public string MedicationName { get; set; }
        public int? PrescriptionId { get; set; }
        public string Dosage { get; set; }
        public int? Quantity { get; set; }
        public int? RemainingQuantity { get; set; }
    }
}
