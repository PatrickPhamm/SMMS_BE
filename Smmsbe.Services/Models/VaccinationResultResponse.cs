using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smmsbe.Services.Models
{
    public class VaccinationResultResponse
    {
        public int VaccinationResultId { get; set; }
        public int? VaccinationScheduleId { get; set; }
        public int? HealthProfileId { get; set; }
        public int? NurseId { get; set; }
        public string NurseName { get; set; }
        public string Status { get; set; }
        public int? DoseNumber { get; set; }
        public string Note { get; set; }
    }
}
