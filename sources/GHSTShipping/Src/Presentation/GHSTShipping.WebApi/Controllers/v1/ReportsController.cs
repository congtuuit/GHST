using Delivery.GHN;
using Delivery.GHN.Models;
using GHSTShipping.Application.Features.Orders.Queries;
using GHSTShipping.Application.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GHSTShipping.WebApi.Controllers.v1
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ReportsController : BaseApiController
    {
        public ReportsController()
        {
        }

        [HttpGet]
        public async Task<BaseResult<IEnumerable<ShopViewReportDto>>> Orders(GetOrderReportsGroupByShopRequest request)
        {
            return await Mediator.Send(request);
        }
    }
}
