using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace GHSTShipping.Infrastructure.Identity.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        [MaxLength(100)]
        public required string Name { get; set; }

        [MaxLength(20)]
        public required string Type { get; set; }

        public DateTime Created { get; set; } = DateTime.UtcNow;
    }
}
