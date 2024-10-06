using System;

namespace GHSTShipping.Application.Interfaces
{
    public interface IAuthenticatedUserService
    {
        string UserId { get; }

        Guid UId
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.UserId))
                {
                    throw new UnauthorizedAccessException();
                }

                return Guid.Parse(UserId);
            }
        }

        string UserName { get; }

        string DisplayName { get; }

        Guid? ShopId { get; }
    }
}
