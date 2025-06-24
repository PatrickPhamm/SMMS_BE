using Smmsbe.Services.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smmsbe.Services.Models
{
    public class AddBlogRequest
    {
        public int? ManagerId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateOnly? DatePosted { get; set; }
        public string Thumbnail { get; set; }
        public BlogCategoryType Category { get; set; }
    }
}
