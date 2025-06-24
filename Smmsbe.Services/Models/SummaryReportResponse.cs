using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smmsbe.Services.Models
{
    public class SummaryReportResponse
    {
        public int TotalParents { get; set; }
        public int TotalNurses { get; set; }
        public int TotalStudents { get; set; }
    }
}
