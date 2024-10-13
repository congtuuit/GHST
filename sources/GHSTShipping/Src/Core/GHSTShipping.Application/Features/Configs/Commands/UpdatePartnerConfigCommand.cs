using GHSTShipping.Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;

namespace GHSTShipping.Application.Features.Configs.Commands
{
    public class UpdatePartnerConfigCommand : IRequest<BaseResult>
    {
        public IEnumerable<UpdatePartnerConfigRequest> Configs { get; set; }
    }

    public class UpdatePartnerConfigRequest
    {
        public Guid Id { get; set; }

        public string ApiKey { get; set; }

        public string UserName { get; set; }

        public bool IsActivated { get; set; }
    }
}
