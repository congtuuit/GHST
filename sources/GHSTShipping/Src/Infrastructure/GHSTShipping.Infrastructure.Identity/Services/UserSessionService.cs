using GHSTShipping.Application.DTOs.Account;
using GHSTShipping.Application.Interfaces.UserInterfaces;
using GHSTShipping.Application.Wrappers;
using GHSTShipping.Infrastructure.Identity.Contexts;
using GHSTShipping.Infrastructure.Identity.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GHSTShipping.Infrastructure.Identity.Services
{
    public class UserSessionService : IUserSessionService
    {
        private readonly IdentityContext _dbContext;

        public UserSessionService(IdentityContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> ValidateUserSessionAsync(Guid userId, string ipAddress)
        {
            return await _dbContext.UserSessions
                .AnyAsync(session => session.UserId == userId &&
                                     session.IpAddress == ipAddress &&
                                     session.IsActive);
        }

        public async Task<UserSessionDto> RegisterUserSessionAsync(Guid userId, string deviceInfo, string ipAddress)
        {
            var session = new UserSession
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                DeviceInfo = deviceInfo,
                IpAddress = ipAddress,
                SessionStart = DateTime.UtcNow,
                LastAccessed = DateTime.UtcNow,
                IsActive = true
            };

            _dbContext.UserSessions.Add(session);
            await _dbContext.SaveChangesAsync();

            return new UserSessionDto
            {
                Id = session.Id,
                UserId = session.UserId,
                DeviceInfo = session.DeviceInfo,
                IpAddress = session.IpAddress,
                SessionStart = DateTime.UtcNow,
                LastAccessed = DateTime.UtcNow,
            };
        }

        public async Task ExpireSessionAsync(Guid sessionId)
        {
            var session = await _dbContext.UserSessions
                .FirstOrDefaultAsync(s => s.Id == sessionId);

            if (session != null)
            {
                session.IsActive = false;
                await _dbContext.SaveChangesAsync();
            }
        }

        // Optional method to expire all sessions for a user
        public async Task ExpireAllSessionsAsync(Guid userId)
        {
            var sessions = await _dbContext.UserSessions
                .Where(s => s.UserId == userId && s.IsActive)
                .ToListAsync();

            foreach (var session in sessions)
            {
                session.IsActive = false;
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task<BaseResult> LogoutSession(Guid sessionId)
        {
            var session = await _dbContext.UserSessions.FindAsync(sessionId);
            if (session != null)
            {
                _dbContext.Attach(session);
                session.IsActive = false;
                await _dbContext.SaveChangesAsync();
            }

            return BaseResult.Ok();
        }

        public async Task<BaseResult<List<UserSessionDto>>> GetActiveSessions(Guid userId)
        {
            var sessions = await _dbContext.UserSessions
                .Where(s => s.UserId == userId && s.IsActive)
                .Select(s => new UserSessionDto
                {
                    Id = s.Id,
                    UserId = s.UserId,
                    DeviceInfo = s.DeviceInfo,
                    IpAddress = s.IpAddress,
                    SessionStart = DateTime.UtcNow,
                    LastAccessed = DateTime.UtcNow,
                })
                .ToListAsync();

            return sessions;
        }

    }
}
