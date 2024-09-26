using GHSTShipping.Application.DTOs;
using GHSTShipping.Domain.Products.DTOs;
using GHSTShipping.Domain.Products.Entities;
using System.Threading.Tasks;

namespace GHSTShipping.Application.Interfaces.Repositories
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<PaginationResponseDto<ProductDto>> GetPagedListAsync(int pageNumber, int pageSize, string name);
    }
}
