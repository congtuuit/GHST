using GHSTShipping.Application.Features.Orders.Commands;
using GHSTShipping.Application.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GHSTShipping.WebApi.Controllers
{
    [AllowAnonymous]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class TriggerController() : BaseApiController
    {
        [HttpGet]
        [Route("SyncOrderGhn")]
        public async Task<BaseResult> SyncOrderGhn([FromQuery] GHN_JobSyncOrderCommand request)
        {
            return await Mediator.Send(request);
        }
    }
}
