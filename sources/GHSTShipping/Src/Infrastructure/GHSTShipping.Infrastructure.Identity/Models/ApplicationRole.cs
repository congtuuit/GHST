using Microsoft.AspNetCore.Identity;
using System;

namespace GHSTShipping.Infrastructure.Identity.Models
{
    public class ApplicationRole(string name) : IdentityRole<Guid>(name)
    {
    }
}
