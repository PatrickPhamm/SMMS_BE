﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smmsbe.Services.Models
{
    public class GetConsultationFormByStudentResponse
    {
        public int ConsultationFormId { get; set; }
        public int? ParentId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string? Status { get; set; }
        public ConsultationScheduleResponse ConsultationSchedule { get; set; }
    }
}
