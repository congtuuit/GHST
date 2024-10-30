using GHSTShipping.Application.DTOs.Account;
using GHSTShipping.Application.Wrappers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GHSTShipping.Application.Interfaces.UserInterfaces
{
    public interface IUserSessionService
    {
        public Task<bool> ValidateUserSessionAsync(Guid userId, string ipAddress);
        public Task<UserSessionDto> RegisterUserSessionAsync(Guid userId, string deviceInfo, string ipAddress);
        public Task ExpireSessionAsync(Guid sessionId);
        public Task<BaseResult<List<UserSessionDto>>> GetActiveSessions(Guid userId);
        public Task<BaseResult> LogoutSession(Guid sessionId);
    }
}
