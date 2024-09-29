using GHSTShipping.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace GHSTShipping.WebApi.Infrastructure.Services
{
    public class AuthenticatedUserService(IHttpContextAccessor httpContextAccessor) : IAuthenticatedUserService
    {
        public string UserId { get; } = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        public string UserName { get; } = httpContextAccessor.HttpContext?.User.Identity?.Name;
        public string DisplayName { get; } = httpContextAccessor.HttpContext?.User.FindFirstValue("DisplayName");
    }
}
