using GHSTShipping.Application.Interfaces.Repositories;
using GHSTShipping.Domain.Entities;
using GHSTShipping.Infrastructure.Persistence.Contexts;

namespace GHSTShipping.Infrastructure.Persistence.Repositories
{
    public class ShopRepository(ApplicationDbContext dbContext) : GenericRepository<Shop>(dbContext), IShopRepository
    {
        /*public async Task<PaginationResponseDto<ProductDto>> GetPagedListAsync(int pageNumber, int pageSize, string name)
        {
            var query = dbContext.Products.OrderBy(p => p.Created).AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(p => p.Name.Contains(name));
            }

            return await Paged(
                query.Select(p => new ProductDto(p)),
                pageNumber,
                pageSize);

        }*/
    }
}
