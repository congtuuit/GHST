using GHSTShipping.Application.DTOs.Account.Requests;
using GHSTShipping.Application.DTOs.Account.Responses;
using GHSTShipping.Application.Wrappers;
using System.Threading.Tasks;

namespace GHSTShipping.Application.Interfaces.UserInterfaces
{
    public interface IAccountServices
    {
        Task<BaseResult<UserDto>> CreateAccountAsync(CreateAccountRequest request);
        Task<BaseResult<string>> RegisterGhostAccount();
        Task<BaseResult> ChangePassword(ChangePasswordRequest model);
        Task<BaseResult> ChangeUserName(ChangeUserNameRequest model);
        Task<BaseResult<AuthenticationResponse>> Authenticate(AuthenticationRequest request);
        Task<BaseResult<AuthenticationResponse>> AuthenticateByUserName(string username);

    }
}
