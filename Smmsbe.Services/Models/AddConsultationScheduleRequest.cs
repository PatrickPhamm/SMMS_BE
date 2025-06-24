using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smmsbe.Services.Models
{
    public class AddConsultationScheduleRequest
    {
        public int? NurseId { get; set; }
        public int? StudentId { get; set; }
        public string Location { get; set; }
        public DateTime? ConsultDate { get; set; }
    }
}
