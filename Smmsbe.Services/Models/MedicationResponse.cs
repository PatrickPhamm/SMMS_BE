using Smmsbe.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smmsbe.Services.Models
{
    public class MedicationResponse
    {
        public int MedicationId { get; set; }
        public int? StudentId { get; set; }
        public string MedicationName { get; set; }
        public string Dosage { get; set; }
        public int? Quantity { get; set; }
        public int? RemainingQuantity { get; set; }
        public ParentPrescriptionResponse Prescription { get; set; }
    }
}
