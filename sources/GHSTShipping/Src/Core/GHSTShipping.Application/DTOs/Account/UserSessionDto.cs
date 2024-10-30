using System;

namespace GHSTShipping.Application.DTOs.Account
{
    public class UserSessionDto
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }  // User's unique identifier

        public string DeviceInfo { get; set; }  // Information about the device (optional)

        public string IpAddress { get; set; }  // IP address from which the user logged in (optional)

        public DateTime SessionStart { get; set; }  // Timestamp of session start

        public DateTime LastAccessed { get; set; }  // Timestamp of last activity in this session

        public bool IsActive { get; set; }  // Indicates if the session is active
    }
}
