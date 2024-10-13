using System;
using System.Threading.Tasks;

namespace GHSTShipping.Application.Interfaces
{
    public interface IOrderCodeSequenceService
    {
        Task CreateOrderCodeSequenceAsync(Guid shopId, string shopUniqueCode, Guid createdById = default);

        Task<long> GenerateOrderCodeAsync(Guid shopId);
    }
}
