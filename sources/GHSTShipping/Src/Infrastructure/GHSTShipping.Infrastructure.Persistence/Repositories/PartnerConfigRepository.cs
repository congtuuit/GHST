using GHSTShipping.Application.Interfaces.Repositories;
using GHSTShipping.Domain.Entities;
using GHSTShipping.Infrastructure.Persistence.Contexts;

namespace GHSTShipping.Infrastructure.Persistence.Repositories
{
    public class PartnerConfigRepository(ApplicationDbContext dbContext) : GenericRepository<PartnerConfig>(dbContext), IPartnerConfigRepository
    {

    }
}
