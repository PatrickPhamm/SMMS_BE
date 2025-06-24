using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smmsbe.Services.Models
{
    public class HealthProfileResponse
    {
        public int HealthProfileId { get; set; }
        public int? StudentId { get; set; }
        public string BloodType { get; set; }
        public string Allergies { get; set; }
    }
}
