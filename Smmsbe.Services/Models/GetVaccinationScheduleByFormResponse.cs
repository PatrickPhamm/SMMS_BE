using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smmsbe.Services.Models
{
    public class GetVaccinationScheduleByFormResponse
    {
        public int VaccinationScheduleId { get; set; }
        public int? ManagerId { get; set; }
        public string Name { get; set; }
        public DateTime? ScheduleDate { get; set; }
        public string Location { get; set; }
        public string Note { get; set; }
        public FormResponse Form { get; set; }
    }
}
