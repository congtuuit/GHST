using Microsoft.EntityFrameworkCore.Storage;
using System.Threading;
using System.Threading.Tasks;

namespace GHSTShipping.Application.Interfaces
{
    public interface IUnitOfWork
    {
        Task<bool> SaveChangesAsync();

        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);

    }
}
