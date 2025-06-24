using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smmsbe.Services.Models
{
    public class HealthCheckScheduleResponse
    {
        public int HealthCheckScheduleId { get; set; }
        public int? FormId { get; set; }
        public int? ManagerId { get; set; }
        public string Name { get; set; }
        public DateTime? CheckDate { get; set; }
        public string Location { get; set; }
        public string Note { get; set; }
    }
}
