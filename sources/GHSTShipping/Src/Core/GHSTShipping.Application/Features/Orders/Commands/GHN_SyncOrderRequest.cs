using Delivery.GHN.Constants;
using GHSTShipping.Application.Wrappers;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GHSTShipping.Application.Features.Orders.Commands
{
    public class GHN_SyncOrderRequest : IRequest<BaseResult>
    {
        public string PartnerOrderCode { get; set; }

        public bool IsSearch { get; set; }

        public SearchOptionDto Option { get; set; }
    }

    public class SearchOptionDto
    {
        public bool AllowSaveAfterSync { get; set; }
        public int ShopId { get; set; } = 0;

        public List<GHN_OrderStatus> Status { get; set; }
        public List<int> PaymentTypeId { get; set; } = new List<int>() { 1, 2, 11, 12 };
        public int FromTime { get; set; }
        public int ToTime { get; set; }
        public int Offset { get; set; }
        public int Limit { get; set; } = 100;
        public string OptionValue { get; set; }
        public int FromCodAmount { get; set; }
        public int ToCodAmount { get; set; } = 1000000000;
        public bool IgnoreShopId { get; set; }
        public List<int> ShopIds { get; set; }
        public bool IsSearchExactly { get; set; } = true;
        public bool? IsPrint { get; set; }
        public bool? IsCodFailedCollected { get; set; }
        public bool? IsDocumentPod { get; set; }
        public string TypeTime { get; set; } = "created_date";
        public string Source { get; set; } = "5sao";
    }

    public class GHN_SyncOrderRequestHandler() : IRequestHandler<GHN_SyncOrderRequest, BaseResult>
    {
        public async Task<BaseResult> Handle(GHN_SyncOrderRequest request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.PartnerOrderCode))
            {

            }

            return BaseResult.Ok();
        }
    }
}
