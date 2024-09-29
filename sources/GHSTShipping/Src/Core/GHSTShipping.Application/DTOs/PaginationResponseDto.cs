using System.Collections.Generic;

namespace GHSTShipping.Application.DTOs
{
    public class PaginationResponseDto<T>(List<T> data, int count, int pageNumber, int pageSize)
    {
        public List<T> Data { get; set; } = data;
        public int Count { get; set; } = count;
        public int PageNumber { get; set; } = pageNumber;
        public int PageSize { get; set; } = pageSize;
    }

    public static class PageSetting
    {
        public static int Page { get; set; } = 1;

        public static int PageSize { get; set; } = 10;
    }
}
