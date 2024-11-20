using GHSTShipping.Application.Interfaces;
using GHSTShipping.Application.Interfaces.Repositories;
using GHSTShipping.Domain.DTOs;
using GHSTShipping.Domain.Entities;
using GHSTShipping.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GHSTShipping.Infrastructure.Persistence.Contexts
{
    public class UnitOfWork(ApplicationDbContext dbContext) : IUnitOfWork
    {
        private IGenericRepository<Shop> _shop;
        private IGenericRepository<ShopPartnerConfig> _shopPartnerConfigs;
        private IGenericRepository<DeliveryPricePlane> _deliveryPricePlanes;
        private IGenericRepository<ShopOrderCodeSequence> _shopOrderCodeSequence;
        private IGenericRepository<Order> _order;
        private IGenericRepository<OrderItem> _orderItems;

        public IGenericRepository<Shop> Shops 
            => _shop ??= new GenericRepository<Shop>(dbContext);

        public IGenericRepository<ShopPartnerConfig> ShopPartnerConfigs
            => _shopPartnerConfigs ??= new GenericRepository<ShopPartnerConfig>(dbContext);

        public IGenericRepository<DeliveryPricePlane> DeliveryPricePlanes
            => _deliveryPricePlanes ??= new GenericRepository<DeliveryPricePlane>(dbContext);
        public IGenericRepository<ShopOrderCodeSequence> ShopOrderCodeSequences 
            => _shopOrderCodeSequence ??= new GenericRepository<ShopOrderCodeSequence>(dbContext);
        public IGenericRepository<Order> Orders 
            => _order ??= new GenericRepository<Order>(dbContext);
        public IGenericRepository<OrderItem> OrderItems
            => _orderItems ??= new GenericRepository<OrderItem>(dbContext);

        public async Task<bool> SaveChangesAsync(CancellationToken cancellation = default)
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

        /// <summary>
        /// Executes the given SQL against the database and returns the number of rows affected.
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<int> ExecuteSqlRawAsync(string sql, params object[] parameters)
        {
            return await dbContext.Database.ExecuteSqlRawAsync(sql, parameters);
        }

        /// <summary>
        /// Creates a LINQ query based on a raw SQL query, which returns a result set of a scalar type natively supported
        /// by the database provider.
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public IQueryable<T> SqlRaw<T>(string sql, params object[] parameters)
        {
            return dbContext.Database.SqlQueryRaw<T>(sql, parameters);
        }
    }
}
