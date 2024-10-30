using System;
using System.ComponentModel.DataAnnotations;

namespace GHSTShipping.Infrastructure.Identity.Models
{
    public class UserSession
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; }  // User's unique identifier

        [MaxLength(256)]
        public string DeviceInfo { get; set; }  // Information about the device (optional)

        [MaxLength(50)]
        public string IpAddress { get; set; }  // IP address from which the user logged in (optional)

        [Required]
        public DateTime SessionStart { get; set; }  // Timestamp of session start

        [Required]
        public DateTime LastAccessed { get; set; }  // Timestamp of last activity in this session

        [Required]
        public bool IsActive { get; set; }  // Indicates if the session is active
    }
}
