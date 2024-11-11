using System;
using System.Collections.Generic;

namespace GHSTShipping.Application.DTOs.Account.Responses
{
    public class AuthenticationResponse
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public IList<string> Roles { get; set; }
        public bool IsVerified { get; set; }
        public string JwToken { get; set; }

        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public Guid? ShopId { get; set; }
    }
}
