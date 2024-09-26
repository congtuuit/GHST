using GHSTShipping.Application.DTOs.Account.Requests;
using GHSTShipping.Application.DTOs.Account.Responses;
using GHSTShipping.Application.Wrappers;
using System.Threading.Tasks;

namespace GHSTShipping.Application.Interfaces.UserInterfaces
{
    public interface IGetUserServices
    {
        Task<PagedResponse<UserDto>> GetPagedUsers(GetAllUsersRequest model);
    }
}
