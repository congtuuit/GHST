using GHSTShipping.Application.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GHSTShipping.Application.Extensions
{
    public static class PagingExtensions
    {
        public static async Task<PaginationResponseDto<T>> ToPaginationAsync<T>(this IQueryable<T> query, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            if (pageNumber == 0) pageNumber = PageSetting.Page;
            if (pageSize == 0) pageSize = PageSetting.PageSize;

            int skip = (pageNumber - 1) * pageSize;
            var pagedResult = await query.Skip(skip).Take(pageSize).ToListAsync();
            var count = await query.CountAsync();

            return new(pagedResult, count, pageNumber, pageSize);
        }
    }
}
