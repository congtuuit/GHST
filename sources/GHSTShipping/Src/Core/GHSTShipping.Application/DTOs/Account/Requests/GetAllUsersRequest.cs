using GHSTShipping.Application.Parameters;

namespace GHSTShipping.Application.DTOs.Account.Requests
{
    public class GetAllUsersRequest : PaginationRequestParameter
    {
        public string Name { get; set; }
    }
}
