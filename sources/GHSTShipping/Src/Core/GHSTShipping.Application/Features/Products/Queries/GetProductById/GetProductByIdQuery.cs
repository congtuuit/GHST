using GHSTShipping.Application.Wrappers;
using GHSTShipping.Domain.Products.DTOs;
using MediatR;

namespace GHSTShipping.Application.Features.Products.Queries.GetProductById
{
    public class GetProductByIdQuery : IRequest<BaseResult<ProductDto>>
    {
        public long Id { get; set; }
    }
}
