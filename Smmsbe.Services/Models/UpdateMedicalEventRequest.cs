using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smmsbe.Services.Models
{
    public class UpdateMedicalEventRequest
    {
        public int EventId { get; set; }
        public int? NurseId { get; set; }
        public DateTime? EventDate { get; set; }
        public string Symptoms { get; set; }
        public string ActionTaken { get; set; }
        public string Note { get; set; }
    }
}
