using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smmsbe.Services.Models
{
    public class UpdateStudentReponse
    {
        public int StudentId { get; set; }
        public string FullName { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public string ClassName { get; set; }
        public string Gender { get; set; }
        public string StudentNumber { get; set; }
    }
}
