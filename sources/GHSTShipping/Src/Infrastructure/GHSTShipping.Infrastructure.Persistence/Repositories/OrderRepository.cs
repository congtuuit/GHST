using GHSTShipping.Application.Interfaces.Repositories;
using GHSTShipping.Domain.Entities;
using GHSTShipping.Infrastructure.Persistence.Contexts;

namespace GHSTShipping.Infrastructure.Persistence.Repositories
{
    public class OrderRepository(ApplicationDbContext dbContext) : GenericRepository<Order>(dbContext), IOrderRepository
    {
    }
}
