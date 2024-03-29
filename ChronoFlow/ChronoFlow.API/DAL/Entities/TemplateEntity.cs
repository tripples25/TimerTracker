﻿using System.ComponentModel.DataAnnotations;
using AutoMapper.Configuration.Annotations;

namespace ChronoFlow.API.DAL.Entities;

public class TemplateEntity : IEntity<TemplateEntity>
{
    [Key]
    public Guid Id { get; set; } = Guid.Empty;
    public string Name { get; set; } = "emptyname";
    [Ignore]
    public virtual List<EventEntity> Events { get; } = new List<EventEntity>();
}