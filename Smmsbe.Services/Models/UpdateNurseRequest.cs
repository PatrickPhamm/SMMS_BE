﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smmsbe.Services.Models
{
    public class UpdateNurseRequest
    {
        public int NurseId { get; set; }
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        //public string PasswordHash { get; set; }
    }
}
