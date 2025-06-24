using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smmsbe.Services.Models
{
    public class GetHealthCheckResultByProfileResponse
    {
        public int HealthCheckupRecordId { get; set; }
        public int? HealthCheckScheduleId { get; set; }
        public int? NurseId { get; set; }
        public string NurseName { get; set; }
        public string Status { get; set; }
        public double? Height { get; set; }
        public double? Weight { get; set; }
        public string LeftVision { get; set; }
        public string RightVision { get; set; }
        public string Result { get; set; }
        public string Note { get; set; }
        public HealthProfileResponse HealthProfile { get; set; }
    }
}
