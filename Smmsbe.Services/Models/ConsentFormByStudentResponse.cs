using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smmsbe.Services.Models
{
    public class ConsentFormByStudentResponse
    {
        public int ConsentFormId { get; set; }
        public int? ParentId { get; set; }
        public int? StudentId { get; set; }
        public string? Status { get; set; }
        public FormResponse Form { get; set; }
    }
}
