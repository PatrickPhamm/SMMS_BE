﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smmsbe.Services.Models
{
    public class UpdateFormRequest
    {
        public int FormId { get; set; }
        public string ClassName { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        //public int Type { get; set; }
    }
}
