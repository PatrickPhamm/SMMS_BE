using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smmsbe.Services.Models
{
    public class MedicalInventoryResponse
    {
        public int MedicalInventoryId { get; set; }
        public int? ManagerId { get; set; }
        public int? Quantity { get; set; }
        public string MedicalName { get; set; }
        public string Unit { get; set; }
        public DateOnly? ExpiryDate { get; set; }
        public DateOnly? DateAdded { get; set; }
    }
}
