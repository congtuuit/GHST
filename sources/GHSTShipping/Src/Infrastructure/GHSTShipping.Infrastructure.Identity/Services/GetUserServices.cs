using GHSTShipping.Application.DTOs;
using GHSTShipping.Application.DTOs.Account.Requests;
using GHSTShipping.Application.DTOs.Account.Responses;
using GHSTShipping.Application.Interfaces.UserInterfaces;
using GHSTShipping.Application.Wrappers;
using GHSTShipping.Infrastructure.Identity.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace GHSTShipping.Infrastructure.Identity.Services
{
    public class GetUserServices(IdentityContext identityContext) : IGetUserServices
    {
        public async Task<PagedResponse<UserDto>> GetPagedUsers(GetAllUsersRequest model)
        {
            var skip = (model.PageNumber - 1) * model.PageSize;

            var users = identityContext.Users
                .Select(p => new UserDto()
                {
                    Name = p.Name,
                    Email = p.Email,
                    PhoneNumber = p.PhoneNumber,
                    Id = p.Id,
                });

            if (!string.IsNullOrEmpty(model.Name))
            {
                users = users.Where(p => p.Name.Contains(model.Name));
            }

            return new PaginationResponseDto<UserDto>(
                await users.Skip(skip).Take(model.PageSize).ToListAsync(),
                await users.CountAsync(),
                model.PageNumber,
                model.PageSize);

        }
    }
}
