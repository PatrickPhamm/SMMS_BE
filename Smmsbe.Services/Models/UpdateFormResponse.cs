using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smmsbe.Services.Models
{
    public class UpdateFormResponse
    {
        public int FormId { get; set; }
        public string ClassName { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Type { get; set; }
    }
}
