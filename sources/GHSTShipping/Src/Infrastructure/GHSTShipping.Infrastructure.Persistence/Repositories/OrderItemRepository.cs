using GHSTShipping.Application.Interfaces.Repositories;
using GHSTShipping.Domain.Entities;
using GHSTShipping.Infrastructure.Persistence.Contexts;

namespace GHSTShipping.Infrastructure.Persistence.Repositories
{
    public class OrderItemRepository(ApplicationDbContext dbContext) : GenericRepository<OrderItem>(dbContext), IOrderItemRepository
    {
    }
}
