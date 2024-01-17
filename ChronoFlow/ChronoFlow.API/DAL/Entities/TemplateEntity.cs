﻿using System.ComponentModel.DataAnnotations;
using AutoMapper.Configuration.Annotations;
using System.Text.Json.Serialization;

namespace ChronoFlow.API.DAL.Entities;

public class TemplateEntity : IEntity<TemplateEntity>
{
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; }
    [Ignore]
    public virtual List<EventEntity> Events { get; } = new List<EventEntity>();
    
    public void UpdateFieldsFromEntity(TemplateEntity? dbEntity)
    {
        dbEntity.Name = Name;
    }

    public void CreateFieldsFromEntity(TemplateEntity? dbEntity)
    {
    }
}