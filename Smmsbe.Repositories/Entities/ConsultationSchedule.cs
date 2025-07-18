﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using Smmsbe.Repositories.Infrastructure;
using System;
using System.Collections.Generic;

namespace Smmsbe.Repositories.Entities;

public partial class ConsultationSchedule : IEntityBase
{
    public int ConsultationScheduleId { get; set; }

    public int? NurseId { get; set; }

    public int? StudentId { get; set; }

    public string Location { get; set; }

    public DateTime? ConsultDate { get; set; }

    public virtual ICollection<ConsultationForm> ConsultationForms { get; set; } = new List<ConsultationForm>();

    public virtual Nurse Nurse { get; set; }

    public virtual Student Student { get; set; }
}