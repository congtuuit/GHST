using GHSTShipping.Application.Interfaces.Repositories;
using GHSTShipping.Domain.Entities;
using GHSTShipping.Infrastructure.Persistence.Contexts;

namespace GHSTShipping.Infrastructure.Persistence.Repositories
{
    public class ShopRepository(ApplicationDbContext dbContext) : GenericRepository<Shop>(dbContext), IShopRepository
    {
    }
}
