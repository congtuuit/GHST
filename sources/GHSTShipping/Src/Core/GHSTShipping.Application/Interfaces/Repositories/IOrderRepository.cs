using GHSTShipping.Domain.Entities;
using System.Collections.Generic;
using System;
using GHSTShipping.Domain.DTOs;
using System.Threading.Tasks;

namespace GHSTShipping.Application.Interfaces.Repositories
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<IEnumerable<SP_OrderCodeResult>> UpdateOrderCodeSequenceAsync(string orderIds, Guid shopId);
    }
}
