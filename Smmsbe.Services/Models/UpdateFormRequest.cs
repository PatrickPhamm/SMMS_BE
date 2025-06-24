using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smmsbe.Services.Models
{
    public class UpdateFormRequest
    {
        public int FormId { get; set; }
        public int? ManagerId { get; set; }
        public string ClassName { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        //public DateTime? SentDate { get; set; }
        //public DateTime? CreatedAt { get; set; }
        //public int Type { get; set; }
    }
}
