using System;

namespace GHSTShipping.Application.DTOs.Account.Responses
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Use for reset password
        /// </summary>
        public string SecurityStamp { get; set; }
    }
}
