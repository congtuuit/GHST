using GHSTShipping.Application.Features.Orders.Queries;
using GHSTShipping.Application.Wrappers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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

        
    }
}
