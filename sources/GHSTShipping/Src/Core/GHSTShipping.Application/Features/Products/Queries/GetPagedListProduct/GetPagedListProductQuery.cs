using GHSTShipping.Application.Parameters;
using GHSTShipping.Application.Wrappers;
using GHSTShipping.Domain.Products.DTOs;
using MediatR;

namespace GHSTShipping.Application.Features.Products.Queries.GetPagedListProduct
{
    public class GetPagedListProductQuery : PaginationRequestParameter, IRequest<PagedResponse<ProductDto>>
    {
        public string Name { get; set; }
    }
}
