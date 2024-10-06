using GHSTShipping.Application.Interfaces.Repositories;
using GHSTShipping.Domain.DTOs;
using GHSTShipping.Domain.Entities;
using GHSTShipping.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GHSTShipping.Infrastructure.Persistence.Repositories
{
    public class OrderRepository(ApplicationDbContext dbContext) : GenericRepository<Order>(dbContext), IOrderRepository
    {
        public async Task<IEnumerable<SP_OrderCodeResult>> UpdateOrderCodeSequenceAsync(string orderIds, Guid shopId)
        {
            return await dbContext.OrderCodeResults
                .FromSqlRaw("EXEC sp_UpdateOrderCodeSequence @OrderIds = {0}, @ShopId = {1}", orderIds, shopId)
                .ToListAsync();
        }
    }
}
