﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smmsbe.Services.Models
{
    public class UpdateBlogRequest
    {
        public int BlogId { get; set; } 
        public string Title { get; set; }
        public string Content { get; set; }
        //public DateOnly? DatePosted { get; set; }
        public string Thumbnail { get; set; }
    }
}
