using GHSTShipping.Application.Interfaces.Repositories;
using GHSTShipping.Domain.Entities;
using Microsoft.EntityFrameworkCore.Storage;
using System.Threading;
using System.Threading.Tasks;

namespace GHSTShipping.Application.Interfaces
{
    public interface IUnitOfWork
    {
        Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);

        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);


        IGenericRepository<Shop> Shops { get; }
    }
}
