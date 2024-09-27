using GHSTShipping.Application.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace GHSTShipping.Infrastructure.Persistence.Contexts
{
    public class UnitOfWork(ApplicationDbContext dbContext) : IUnitOfWork
    {
        public async Task<bool> SaveChangesAsync()
        {
            return await dbContext.SaveChangesAsync() > 0;
        }

        public bool SaveChanges()
        {
            return dbContext.SaveChanges() > 0;
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            return await dbContext.Database.BeginTransactionAsync(cancellationToken);
        }
    }
}
