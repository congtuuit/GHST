using GHSTShipping.Infrastructure.Identity.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GHSTShipping.WebApi.Middlewares
{
    public class SessionMiddleware
    {
        private readonly RequestDelegate _next;

        public SessionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IdentityContext dbContext)
        {
            if (context.User.Identity.IsAuthenticated)
            {
                var userId = new Guid(context.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var session = await dbContext.UserSessions.FirstOrDefaultAsync(s => s.UserId == userId && s.IsActive);

                if (session != null)
                {
                    dbContext.Attach(session);
                    session.LastAccessed = DateTime.UtcNow;

                    await dbContext.SaveChangesAsync();
                }
            }

            await _next(context);
        }
    }
}
