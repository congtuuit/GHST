using GHSTShipping.Application.DTOs.Account.Requests;
using GHSTShipping.Application.DTOs.Account.Responses;
using GHSTShipping.Application.Wrappers;
using System;
using System.Threading.Tasks;

namespace GHSTShipping.Application.Interfaces.UserInterfaces
{
    public interface IAccountServices
    {
        Task<BaseResult> SignOutAsync();

        /// <summary>
        /// Create account for shop
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<BaseResult<UserDto>> CreateAccountAsync(CreateAccountRequest request);

        Task<BaseResult<string>> RegisterGhostAccountAsync();

        Task<BaseResult> ChangePasswordAsync(ChangePasswordRequest model);

        Task<BaseResult> ChangeUserNameAsync(ChangeUserNameRequest model);

        Task<BaseResult<AuthenticationResponse>> Authenticate(AuthenticationRequest request);

        Task<BaseResult<AuthenticationResponse>> AuthenticateByUserNameAsync(string username);

        [Obsolete]
        Task<BaseResult> SetPasswordViaSecurityStampAsync(string securityStamp, string password);

        Task<BaseResult> HandleSendEmailToSetPasswrodAsync(string email);

        Task<BaseResult> HandleSendEmailForgotPasswordAsync(ForgotPasswordRequest request);

        Task<BaseResult> ResetPasswordAsync(string token, string email, string newPassword);
    }
}
