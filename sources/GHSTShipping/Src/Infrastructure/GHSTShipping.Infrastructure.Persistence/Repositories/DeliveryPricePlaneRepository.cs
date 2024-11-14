using GHSTShipping.Application.Interfaces.Repositories;
using GHSTShipping.Domain.Entities;
using GHSTShipping.Infrastructure.Persistence.Contexts;

namespace GHSTShipping.Infrastructure.Persistence.Repositories
{
    public class DeliveryPricePlaneRepository(ApplicationDbContext dbContext) : GenericRepository<DeliveryPricePlane>(dbContext), IDeliveryPricePlaneRepository
    {
    }
}
