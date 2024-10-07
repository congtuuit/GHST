using GHSTShipping.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;

namespace GHSTShipping.WebApi.Infrastructure.Services
{
    public class AuthenticatedUserService(IHttpContextAccessor httpContextAccessor) : IAuthenticatedUserService
    {
        public string UserId { get; } = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        public string UserName { get; } = httpContextAccessor.HttpContext?.User.Identity?.Name;
        public string DisplayName { get; } = httpContextAccessor.HttpContext?.User.FindFirstValue("DisplayName");
        public string Type { get; } = httpContextAccessor.HttpContext?.User.FindFirstValue("Type");
        public Guid? ShopId
        {
            get
            {
                string value = httpContextAccessor.HttpContext?.User.FindFirstValue("ShopId");
                if (string.IsNullOrWhiteSpace(value))
                {
                    return null;
                }
                else
                {
                    return Guid.Parse(value);
                }
            }
        }
    }
}
