using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smmsbe.Services.Models
{
    public class AddVaccinationResultRequest
    {
        public int? VaccinationScheduleId { get; set; }
        public int? HealthProfileId { get; set; }
        public int? NurseId { get; set; }
        public int? DoseNumber { get; set; }
        public string Note { get; set; }
    }
}
