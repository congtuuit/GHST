using AutoMapper;
using Delivery.GHN;
using GHSTShipping.Application.Interfaces;
using GHSTShipping.Application.Interfaces.Repositories;
using GHSTShipping.Application.Wrappers;
using GHSTShipping.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GHSTShipping.Application.Features.Orders.Commands
{
    public class GHN_ConfirmOrderRequest : IRequest<BaseResult>
    {
        public Guid OrderId { get; set; }
    }

    public class ConfirmOrderGhnRequestHandler(
        IGhnApiClient ghnApiClient,
        IPartnerConfigService partnerConfigService,
        IAuthenticatedUserService authenticatedUserService,
        IShopRepository shopRepository,
        IOrderRepository orderRepository,
        IOrderHistoryRepository orderHistoryRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IMediator mediator
        ) : IRequestHandler<GHN_ConfirmOrderRequest, BaseResult>
    {
        public async Task<BaseResult> Handle(GHN_ConfirmOrderRequest request, CancellationToken cancellationToken)
        {
            var order = await orderRepository.Where(i => request.OrderId == i.Id)
                .Select(i => new Domain.Entities.Order
                {
                    Id = i.Id,
                    CurrentStatus = i.CurrentStatus,
                    PartnerShopId = i.PartnerShopId,

                    PickShift = i.PickShift,
                    ServiceTypeId = i.ServiceTypeId,
                    FromPhone = i.FromPhone,
                    FromName = i.FromName,
                    FromProvinceId = i.FromProvinceId,
                    FromProvinceName = i.FromProvinceName,
                    FromAddress = i.FromAddress,
                    FromDistrictId = i.FromDistrictId,
                    FromDistrictName = i.FromDistrictName,
                    FromWardId = i.FromWardId,
                    FromWardName = i.FromWardName,
                    ToPhone = i.ToPhone,
                    ToName = i.ToName,
                    ToProvinceId = i.ToProvinceId,
                    ToProvinceName = i.ToProvinceName,
                    ToAddress = i.ToAddress,
                    ToDistrictId = i.ToDistrictId,
                    ToDistrictName = i.ToDistrictName,
                    ToWardId = i.ToWardId,
                    ToWardName = i.ToWardName,
                    Weight = i.Weight,
                    Length = i.Length,
                    Width = i.Width,
                    Height = i.Height,
                    CodAmount = i.CodAmount,
                    InsuranceValue = i.InsuranceValue,
                    RequiredNote = i.RequiredNote,
                    Note = i.Note,
                    Content = i.Content,
                    PaymentTypeId = i.PaymentTypeId,
                    ClientOrderCode = i.ClientOrderCode,
                    ReturnPhone = i.ReturnPhone,
                    ReturnAddress = i.ReturnAddress,
                    ReturnDistrictName = i.ReturnDistrictName,
                    ReturnWardName = i.ReturnWardName,
                    PickStationId = i.PickStationId,
                    DeliverStationId = i.DeliverStationId,
                    ServiceId = i.ServiceId,
                    Coupon = i.Coupon,
                    Items = i.Items.Select(oi => new OrderItem
                    {
                        Id = oi.Id,
                        Name = oi.Name,
                        Quantity = oi.Quantity,
                        Code = oi.Code,
                        Price = oi.Price,
                        Length = oi.Length,
                        Width = oi.Width,
                        Height = oi.Height,
                        Weight = oi.Weight,
                    }).ToList(),
                })
                .FirstOrDefaultAsync();

            if (order == null)
            {
                return BaseResult.Failure(new Error(ErrorCode.NotFound, "Không tìm thấy đơn hàng"));
            }

            try
            {
                // Send order to GHN
                var createDeliveryOrderRequest = mapper.Map<CreateDeliveryOrderRequest>(order);
                var apiConfig = await partnerConfigService.GetApiConfigAsync(Domain.Enums.EnumDeliveryPartner.GHN, order.ShopId.Value);
                var createOrder = await ghnApiClient.CreateDeliveryOrderAsync(apiConfig, order.PartnerShopId, createDeliveryOrderRequest);
                if (createOrder.Code == 200)
                {
                    var orderPublished = createOrder.Data;

                    // Duplicated: update order from parter
                    orderRepository.Modify(order);
                    order.Publish();
                    order.PrivateUpdateFromPartner(
                        private_order_code: orderPublished.order_code,
                        private_sort_code: orderPublished.sort_code,
                        private_trans_type: orderPublished.trans_type,
                        private_total_fee: orderPublished.total_fee,
                        private_expected_delivery_time: orderPublished.expected_delivery_time,
                        private_operation_partner: orderPublished.operation_partner
                    );

                    await orderHistoryRepository.AddAsync(new OrderStatusHistory
                    {
                        OrderId = order.Id,
                        Status = OrderStatus.READY_TO_PICK,
                        ChangedBy = authenticatedUserService.UserId,
                        Notes = "Publish order"
                    });

                    await unitOfWork.SaveChangesAsync(cancellationToken);

                    await mediator.Send(new GHN_SyncOrderRequest
                    {
                        ShopId = order.ShopId.Value,
                        PartnerOrderCode = order.private_order_code
                    },
                    cancellationToken);

                    return BaseResult.Ok();
                }
                else
                {
                    return BaseResult.Failure(new Error(ErrorCode.AccessDenied, "Lỗi kết nối với đơn vị vận chuyển, vui lòng kiểm tra lại!"));
                }
            }
            catch (Exception ex)
            {
            }

            return BaseResult.Failure(new Error(ErrorCode.Exception, "Xảy ra lỗi, vui lòng thử lại!"));
        }
    }
}
