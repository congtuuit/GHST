using GHSTShipping.Application.Wrappers;
using MediatR;

namespace GHSTShipping.Application.Features.Products.Commands.DeleteProduct
{
    public class DeleteProductCommand : IRequest<BaseResult>
    {
        public long Id { get; set; }
    }
}
