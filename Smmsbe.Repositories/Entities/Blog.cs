﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using Smmsbe.Repositories.Infrastructure;
using System;
using System.Collections.Generic;

namespace Smmsbe.Repositories.Entities;

public partial class Blog : IEntityBase
{
    public int BlogId { get; set; }

    public int? ManagerId { get; set; }

    public string Title { get; set; }

    public string Content { get; set; }

    public DateOnly? DatePosted { get; set; }

    public string Thumbnail { get; set; }

    public int? Category { get; set; }

    public virtual Manager Manager { get; set; }
}