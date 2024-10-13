using GHSTShipping.Application.Interfaces;
using GHSTShipping.Domain.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace GHSTShipping.Application.Services
{
    public class OrderCodeSequenceService(IUnitOfWork unitOfWork) : IOrderCodeSequenceService
    {
        public async Task CreateOrderCodeSequenceAsync(Guid shopId, string shopUniqueCode, Guid createdById = default)
        {
            await unitOfWork.ExecuteSqlRawAsync("EXEC CreateOrderCodeSequenceAndRecord @p0, @p1, @p2", shopId, shopUniqueCode, createdById);
        }

        public async Task<long> GenerateOrderCodeAsync(Guid shopId)
        {
            // Retrieve the sequence name directly
            var sequenceName = await unitOfWork.ShopOrderCodeSequences
                .Where(i => i.ShopId == shopId)
                .Select(i => i.SequenceName)
                .FirstOrDefaultAsync() ?? throw new Exception("Sequence not found for the specified shop.");

            // Construct SQL query to fetch the next value for the sequence
            string sequenceSelectSql = $"SELECT NEXT VALUE FOR [{sequenceName}] AS NextValue";

            // Use the Database.ExecuteSqlRaw to execute the query and fetch the next value directly
            var result = await unitOfWork
               .SqlRaw<OrderCodeSequenceResult>(sequenceSelectSql)
               .ToArrayAsync();

            return result.FirstOrDefault()?.NextValue ?? 0;
        }
    }
}
