using GHSTShipping.Application.Interfaces.Repositories;
using GHSTShipping.Domain.Entities;
using GHSTShipping.Infrastructure.Persistence.Contexts;

namespace GHSTShipping.Infrastructure.Persistence.Repositories
{
    public class OrderHistoryRepository(ApplicationDbContext dbContext) : GenericRepository<OrderStatusHistory>(dbContext), IOrderHistoryRepository
    {
    }
}
