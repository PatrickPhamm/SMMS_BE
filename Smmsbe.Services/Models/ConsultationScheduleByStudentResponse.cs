using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smmsbe.Services.Models
{
    public class ConsultationScheduleByStudentResponse
    {
        public int ConsultationScheduleId { get; set; }
        public int? NurseId { get; set; }
        public string Location { get; set; }
        public DateTime? ConsultDate { get; set; }
        public StudentResponse Student { get; set; }
    }
}
