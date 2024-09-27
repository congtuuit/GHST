using System;
using System.ComponentModel.DataAnnotations;

namespace GHSTShipping.Domain.Common
{
    public abstract class BaseEntity
    {
        [Key]
        public Guid Id { get; set; } = new Microsoft.EntityFrameworkCore.ValueGeneration.SequentialGuidValueGenerator().Next(null);
    }
}
