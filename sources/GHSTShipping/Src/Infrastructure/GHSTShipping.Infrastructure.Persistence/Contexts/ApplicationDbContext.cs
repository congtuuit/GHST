using GHSTShipping.Application.Interfaces;
using GHSTShipping.Domain.Common;
using GHSTShipping.Domain.Entities;
using GHSTShipping.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace GHSTShipping.Infrastructure.Persistence.Contexts
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IAuthenticatedUserService authenticatedUser) : DbContext(options)
    {
        public DbSet<CodeSequence> CodeSequences { get; set; }
        public DbSet<Shop> Shops { get; set; }
        public DbSet<ShopPricePlan> ShopPricePlanes { get; set; }
        public DbSet<PartnerConfig> PartnerConfigs { get; set; }


        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var userId = string.IsNullOrEmpty(authenticatedUser.UserId)
                ? Guid.Empty : Guid.Parse(authenticatedUser.UserId);

            var currentTime = DateTime.UtcNow;

            foreach (var entry in ChangeTracker.Entries<AuditableBaseEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.Created = currentTime;
                    entry.Entity.CreatedBy = userId;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.LastModified = currentTime;
                    entry.Entity.LastModifiedBy = userId;
                }
                /*else if (entry.State == EntityState.Deleted)
                {
                    entry.State = EntityState.Modified;
                    entry.Entity.LastModified = currentTime;
                    entry.Entity.LastModifiedBy = userId;

                    entry.CurrentValues["IsDeleted"] = true;
                }*/
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //All Decimals will have 18,6 Range
            foreach (var property in builder.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetColumnType("decimal(18,2)");
            }
            builder.ApplyConfigurationsFromAssembly(GetType().Assembly);

            // Filter with is delete field
            Expression<Func<AuditableBaseEntity, bool>> filterExpr = x => !x.IsDeleted;
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                // Check if current entity type is child of BaseEntity
                if (entityType.ClrType.IsAssignableTo(typeof(AuditableBaseEntity)))
                {
                    // Modify expression to handle correct child type
                    var parameter = Expression.Parameter(entityType.ClrType);
                    var body = ReplacingExpressionVisitor.Replace(filterExpr.Parameters.First(), parameter, filterExpr.Body);
                    var lambdaExpression = Expression.Lambda(body, parameter);

                    // Set filter
                    entityType.SetQueryFilter(lambdaExpression);
                }
            }


            base.OnModelCreating(builder);
        }
    }
}
