using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smmsbe.Services.Models
{
    public class LoginStudentRequest
    {
        [Required]
        public string StudentNumber { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
