﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using Smmsbe.Repositories.Infrastructure;
using System;
using System.Collections.Generic;

namespace Smmsbe.Repositories.Entities;

public partial class Medication : IEntityBase
{
    public int MedicationId { get; set; }

    public int? StudentId { get; set; }

    public string MedicationName { get; set; }

    public int? PrescriptionId { get; set; }

    public string Dosage { get; set; }

    public int? Quantity { get; set; }

    public int? RemainingQuantity { get; set; }

    public virtual ParentPrescription Prescription { get; set; }

    public virtual Student Student { get; set; }
}