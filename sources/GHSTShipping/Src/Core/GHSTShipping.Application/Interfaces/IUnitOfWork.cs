using GHSTShipping.Application.Interfaces.Repositories;
using GHSTShipping.Domain.Entities;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GHSTShipping.Application.Interfaces
{
    public interface IUnitOfWork
    {
        Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);

        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);

        Task<int> ExecuteSqlRawAsync(string sql, params object[] parameters);

        IQueryable<T> SqlRaw<T>(string sql, params object[] parameters);

        IGenericRepository<Shop> Shops { get; }
        IGenericRepository<ShopPricePlan> ShopPricePlanes { get; }
        IGenericRepository<ShopOrderCodeSequence> ShopOrderCodeSequences { get; }
        IGenericRepository<Order> Orders { get; }
        IGenericRepository<OrderItem> OrderItems { get; }
    }
}
