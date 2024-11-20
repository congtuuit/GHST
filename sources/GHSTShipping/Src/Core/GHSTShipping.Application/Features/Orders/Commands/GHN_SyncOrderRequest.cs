using Delivery.GHN;
using Delivery.GHN.Models;
using GHSTShipping.Application.Helpers;
using GHSTShipping.Application.Interfaces;
using GHSTShipping.Application.Interfaces.Repositories;
using GHSTShipping.Application.Wrappers;
using GHSTShipping.Domain.Entities;
using GHSTShipping.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GHSTShipping.Application.Features.Orders.Commands
{
    public class GHN_SyncOrderRequest : IRequest<BaseResult>
    {
        public Guid ShopId { get; set; }

        public string PartnerOrderCode { get; set; }
    }

    public class GHN_SyncOrderRequestHandler(
        IUnitOfWork unitOfWork,
        IGhnApiClient ghnApiClient,
        IPartnerConfigService partnerConfigService,
        IShopRepository shopRepository
        ) : IRequestHandler<GHN_SyncOrderRequest, BaseResult>
    {
        const int BATCH_SIZE = 100; // Define the batch size to process at a time

        public async Task<BaseResult> Handle(GHN_SyncOrderRequest request, CancellationToken cancellationToken)
        {
            var apiConfig = await partnerConfigService.GetApiConfigAsync(Domain.Enums.EnumDeliveryPartner.GHN, request.ShopId);
            var ghnOrdersResponse = await ghnApiClient.SearchOrdersAsync(apiConfig, new Delivery.GHN.Models.ShippingOrderSearchRequest
            {
                ShopId = int.Parse(apiConfig.ShopId),
                OptionValue = request.PartnerOrderCode
            });

            var shopUniqueCode = await shopRepository.Where(i => i.Id == request.ShopId)
                    .Select(i => i.UniqueCode)
                    .FirstOrDefaultAsync(cancellationToken);

            var (orders, orderItems) = await ListOrderMappingAsync(ghnOrdersResponse, request.ShopId, shopUniqueCode, apiConfig, ghnApiClient);
            await BatchSaveAsync(unitOfWork, orders, orderItems);

            return BaseResult.Ok();
        }

        public static async Task<(List<Order>, List<OrderItem>)> ListOrderMappingAsync(
            SearchOrderResponse request,
            Guid shopId,
            string shopUniqueCode,
            ApiConfig apiConfig,
            IGhnApiClient _ghnApiClient
            )
        {
            var dictricts = await JsonFileHelper.GetDeliveryAddressAsync<DistrictResponse>();

            var ghnOrders = request.Data;
            var socs = request.Soc;

            var entityOrders = new List<Order>();
            var entityOrderItems = new List<OrderItem>();

            for (int i = 0; i < ghnOrders.Count; i++)
            {
                var o = ghnOrders[i];

                // những đơn là cancel sẽ không đính kèm client code, nên phải lấy về hết
                if (o.Status != OrderStatus.CANCEL)
                {
                    string clientOrderCode = o.ClientOrderCode;
                    if (string.IsNullOrWhiteSpace(clientOrderCode)) continue; // Ignore the order has not client order code
                    if (!clientOrderCode.Contains(shopUniqueCode)) continue; // Ignore the order not match shop unique code
                }

                var deliveryFee = 0; // NEED UPDATE LOGIC
                int privateDeliveryFee = 0; // TODO calc fro soc

                string private_order_code = o.OrderCode;
                string private_sort_code = o.SortCode;
                string private_trans_type = "";
                int private_total_fee = 0;
                DateTime private_expected_delivery_time = DateTime.UtcNow;
                string private_operation_partner = "";

                var soc = socs.FirstOrDefault(i => i.Id == o.SocId && i.OrderCode == o.OrderCode);
                if (soc != null)
                {
                    var payment = soc.Payment;
                    var paymentValue = soc.Payment.Sum(i => i.Value);

                    privateDeliveryFee = paymentValue;
                    private_total_fee = paymentValue;

                    // set delivery fee
                    deliveryFee = paymentValue;
                }

                var entityOrder = OrderMapper(o, shopId, deliveryFee);

                var returnDistrict = dictricts.FirstOrDefault(i => $"{i.DistrictID}" == entityOrder.ReturnDistrictId);
                if (returnDistrict != null)
                {
                    entityOrder.ReturnDistrictName = returnDistrict.DistrictName;
                    var response = await _ghnApiClient.GetWardAsync(apiConfig, returnDistrict.DistrictID);
                    var returnWard = response.FirstOrDefault(i => i.WardCode == entityOrder.FromWardId);
                    if (returnWard != null)
                    {
                        entityOrder.ReturnWardName = returnWard.WardName;
                    }
                }

                var fromDistrict = dictricts.FirstOrDefault(i => i.DistrictID == entityOrder.FromDistrictId);
                if (fromDistrict != null)
                {
                    entityOrder.FromDistrictId = fromDistrict.DistrictID;
                    entityOrder.FromDistrictName = fromDistrict.DistrictName;
                    entityOrder.FromProvinceId = fromDistrict.ProvinceID;
                    entityOrder.FromProvinceName = fromDistrict.ProvinceName;

                    var response = await _ghnApiClient.GetWardAsync(apiConfig, fromDistrict.DistrictID);
                    var fromWard = response.FirstOrDefault(i => i.WardCode == entityOrder.FromWardId);
                    if (fromWard != null)
                    {
                        entityOrder.FromWardName = fromWard.WardName;
                    }
                }

                var toDistrict = dictricts.FirstOrDefault(i => i.DistrictID == entityOrder.ToDistrictId);
                if (toDistrict != null)
                {
                    entityOrder.ToDistrictId = toDistrict.DistrictID;
                    entityOrder.ToDistrictName = toDistrict.DistrictName;
                    entityOrder.ToProvinceId = toDistrict.ProvinceID;
                    entityOrder.ToProvinceName = toDistrict.ProvinceName;

                    var response = await _ghnApiClient.GetWardAsync(apiConfig, toDistrict.DistrictID);
                    var toWard = response.FirstOrDefault(i => i.WardCode == entityOrder.ToWardId);
                    if (toWard != null)
                    {
                        entityOrder.ToWardName = toWard.WardName;
                    }
                }

                entityOrder.PrivateUpdateFromPartner(
                    private_order_code,
                    private_sort_code,
                    private_trans_type,
                    private_total_fee,
                    private_expected_delivery_time,
                    private_operation_partner);

                entityOrders.Add(entityOrder);

                var items = o.Items.Select(i =>
                {
                    OrderItem item = OrderItemMapper(i);
                    item.OrderId = entityOrder.Id;
                    return item;
                })
                .ToList();

                entityOrderItems.AddRange(items);
            }

            return (entityOrders, entityOrderItems);
        }

        public static async Task<List<Guid>> BatchSaveAsync(
            IUnitOfWork unitOfWork,
            List<Order> entityOrders,
            List<OrderItem> entityOrderItems,
            int batchSize = BATCH_SIZE
        )
        {
            // OrderIds processed
            var response = new List<Guid>();

            // Initialize lists to hold new and update batches
            var newOrderBatch = new List<Order>();
            var updateOrderBatch = new List<Order>();
            var newOrderItemBatch = new List<OrderItem>();
            var updateOrderItemBatch = new List<OrderItem>();

            // Fetch existing orders based on private_order_code
            var orderCodes = entityOrders.Select(o => o.private_order_code).Distinct();
            var existingOrders = await unitOfWork.Orders
                .Where(o => orderCodes.Contains(o.private_order_code))
                .AsNoTracking() // Thêm AsNoTracking() ở đây
                .Select(o => new Order
                {
                    Id = o.Id,
                    ClientOrderCode = o.ClientOrderCode,
                    private_order_code = o.private_order_code,
                })
                .ToDictionaryAsync(o => o.private_order_code);

            // Fetch existing order items in a single query
            var existingOrderItemCodes = entityOrderItems
                .Select(item => item.ItemOrderCode)
                .Distinct();

            var existingOrderItems = await unitOfWork.OrderItems
                .Where(item => existingOrderItemCodes.Contains(item.ItemOrderCode))
                .Select(oi => new OrderItem
                {
                    Id = oi.Id,
                    OrderId = oi.OrderId,
                    Code = oi.Code,
                    ItemOrderCode = oi.ItemOrderCode,
                })
                .ToListAsync();

            for (int i = 0; i < entityOrders.Count; i++)
            {
                var currentOrder = entityOrders[i];

                // Process each related item for the current order
                var relatedItems = entityOrderItems.Where(item => item.OrderId == currentOrder.Id).ToList();

                // Check if current order exists
                if (existingOrders.TryGetValue(currentOrder.private_order_code, out var existingOrder))
                {
                    // Hold existed data
                    currentOrder.Id = existingOrder.Id;

                    unitOfWork.Orders.Modify(existingOrder);
                    existingOrder.ReturnName = currentOrder.ReturnName;
                    existingOrder.ReturnPhone = currentOrder.ReturnPhone;
                    existingOrder.ReturnAddress = currentOrder.ReturnAddress;
                    existingOrder.ReturnDistrictId = currentOrder.ReturnDistrictId;
                    existingOrder.ReturnDistrictName = currentOrder.ReturnDistrictName;
                    existingOrder.ReturnWardCode = currentOrder.ReturnWardCode;
                    existingOrder.ReturnWardName = currentOrder.ReturnWardName;
                    existingOrder.FromProvinceName = currentOrder.FromProvinceName;
                    existingOrder.Content = currentOrder.Content;
                    existingOrder.PickStationId = currentOrder.PickStationId;
                    existingOrder.DeliverStationId = currentOrder.DeliverStationId;
                    existingOrder.Coupon = currentOrder.Coupon;
                    existingOrder.CurrentStatus = currentOrder.CurrentStatus;
                    existingOrder.LastSyncDate = DateTime.UtcNow;

                    existingOrder.PrivateUpdateFromPartner(
                        currentOrder.private_order_code,
                        currentOrder.private_sort_code,
                        currentOrder.private_trans_type,
                        currentOrder.private_total_fee,
                        currentOrder.private_expected_delivery_time,
                        currentOrder.private_operation_partner
                    );

                    response.Add(existingOrder.Id);
                }
                else
                {
                    newOrderBatch.Add(currentOrder);

                    response.Add(currentOrder.Id);
                }

                foreach (var item in relatedItems)
                {
                    item.OrderId = currentOrder.Id;

                    // Check if item exists
                    var existingItem = existingOrderItems.FirstOrDefault(i => i.ItemOrderCode == item.ItemOrderCode);
                    if (existingItem != null)
                    {
                        item.Id = existingItem.Id;
                        updateOrderItemBatch.Add(item);
                    }
                    else
                    {
                        newOrderItemBatch.Add(item);
                    }
                }

                // Process batch if batch size is reached or last item
                if (newOrderBatch.Count > 0 || newOrderItemBatch.Count > 0)
                {
                    // Add new orders and items in bulk
                    if (newOrderBatch.Count > 0)
                    {
                        await unitOfWork.Orders.AddRangeAsync(newOrderBatch);
                    }

                    if (newOrderItemBatch.Count > 0)
                    {
                        await unitOfWork.OrderItems.AddRangeAsync(newOrderItemBatch);
                    }

                    // Commit the changes to the database
                    await unitOfWork.SaveChangesAsync();

                    // Clear lists for next batch
                    newOrderBatch.Clear();
                    newOrderItemBatch.Clear();
                }

                if (updateOrderItemBatch.Count > 0)
                {
                    foreach (var item in updateOrderItemBatch)
                    {
                        unitOfWork.OrderItems.Modify(item);
                    }

                    unitOfWork.OrderItems.UpdateRange(updateOrderItemBatch);
                    updateOrderItemBatch.Clear();
                }

                // Commit the changes to the database
                await unitOfWork.SaveChangesAsync();
            }

            return response;
        }

        public static async Task<List<Guid>> NEW_BatchSaveAsync(
            IUnitOfWork unitOfWork,
            List<Order> entityOrders,
            List<OrderItem> entityOrderItems,
            int batchSize = BATCH_SIZE)
        {
            // OrderIds processed
            var response = new List<Guid>();

            // Initialize lists to hold new and update batches
            var newOrderBatch = new List<Order>();
            var updateOrderBatch = new List<Order>();
            var newOrderItemBatch = new List<OrderItem>();
            var updateOrderItemBatch = new List<OrderItem>();

            // Fetch existing orders based on private_order_code
            var existingOrders = await FetchExistingOrdersAsync(unitOfWork, entityOrders);

            // Fetch existing order items in a single query
            var existingOrderItems = await FetchExistingOrderItemsAsync(unitOfWork, entityOrderItems);

            // Loop through each order to categorize them into new or update batches
            for (int i = 0; i < entityOrders.Count; i++)
            {
                var currentOrder = entityOrders[i];
                var relatedItems = entityOrderItems.Where(item => item.OrderId == currentOrder.Id).ToList();

                if (existingOrders.TryGetValue(currentOrder.private_order_code, out var existingOrder))
                {
                    // Update existing order
                    PrepareOrderForUpdate(existingOrder, currentOrder);
                    updateOrderBatch.Add(existingOrder);
                    response.Add(existingOrder.Id);
                }
                else
                {
                    // Add to new order batch
                    newOrderBatch.Add(currentOrder);
                    response.Add(currentOrder.Id);
                }

                // Categorize OrderItems into new or update batches
                foreach (var item in relatedItems)
                {
                    item.OrderId = currentOrder.Id;
                    var existingItem = existingOrderItems.FirstOrDefault(i => i.ItemOrderCode == item.ItemOrderCode);

                    if (existingItem != null)
                    {
                        item.Id = existingItem.Id;
                        item.OrderId = existingItem.OrderId;
                        updateOrderItemBatch.Add(item);
                    }
                    else
                    {
                        newOrderItemBatch.Add(item);
                    }
                }
            }

            // Save Orders and OrderItems in batches
            await SaveOrdersAsync(unitOfWork, newOrderBatch, updateOrderBatch);
            await SaveOrderItemsAsync(unitOfWork, newOrderItemBatch, updateOrderItemBatch);

            return response;
        }

        private static async Task<Dictionary<string, Order>> FetchExistingOrdersAsync(
            IUnitOfWork unitOfWork, List<Order> entityOrders)
        {
            var orderCodes = entityOrders.Select(o => o.private_order_code).Distinct();
            return await unitOfWork.Orders
                .Where(o => orderCodes.Contains(o.private_order_code))
                .AsNoTracking()
                .ToDictionaryAsync(o => o.private_order_code);
        }


        private static async Task<List<OrderItem>> FetchExistingOrderItemsAsync(
            IUnitOfWork unitOfWork, List<OrderItem> entityOrderItems)
        {
            var existingOrderItemCodes = entityOrderItems
                .Select(item => item.ItemOrderCode)
                .Distinct();

            return await unitOfWork.OrderItems
                .Where(item => existingOrderItemCodes.Contains(item.ItemOrderCode))
                .AsNoTracking()
                .ToListAsync();
        }

        private static void PrepareOrderForUpdate(Order existingOrder, Order currentOrder)
        {
            existingOrder.ReturnName = currentOrder.ReturnName;
            existingOrder.ReturnPhone = currentOrder.ReturnPhone;
            existingOrder.ReturnAddress = currentOrder.ReturnAddress;
            existingOrder.ReturnDistrictId = currentOrder.ReturnDistrictId;
            existingOrder.ReturnDistrictName = currentOrder.ReturnDistrictName;
            existingOrder.ReturnWardCode = currentOrder.ReturnWardCode;
            existingOrder.ReturnWardName = currentOrder.ReturnWardName;
            existingOrder.FromProvinceName = currentOrder.FromProvinceName;
            existingOrder.Content = currentOrder.Content;
            existingOrder.PickStationId = currentOrder.PickStationId;
            existingOrder.DeliverStationId = currentOrder.DeliverStationId;
            existingOrder.Coupon = currentOrder.Coupon;
            existingOrder.CurrentStatus = currentOrder.CurrentStatus;
            existingOrder.LastSyncDate = DateTime.UtcNow;

            existingOrder.PrivateUpdateFromPartner(
                currentOrder.private_order_code,
                currentOrder.private_sort_code,
                currentOrder.private_trans_type,
                currentOrder.private_total_fee,
                currentOrder.private_expected_delivery_time,
                currentOrder.private_operation_partner);
        }

        private static async Task SaveOrdersAsync(
            IUnitOfWork unitOfWork,
            List<Order> newOrders,
            List<Order> updateOrders)
        {
            // Add new orders
            if (newOrders.Count > 0)
            {
                await unitOfWork.Orders.AddRangeAsync(newOrders);
                await unitOfWork.SaveChangesAsync();
            }

            // Update existing orders
            if (updateOrders.Count > 0)
            {
                unitOfWork.Orders.UpdateRange(updateOrders);
                await unitOfWork.SaveChangesAsync();
            }
        }


        private static async Task SaveOrderItemsAsync(
            IUnitOfWork unitOfWork,
            List<OrderItem> newOrderItems,
            List<OrderItem> updateOrderItems)
        {
            // Add new order items
            if (newOrderItems.Count > 0)
            {
                await unitOfWork.OrderItems.AddRangeAsync(newOrderItems);
                await unitOfWork.SaveChangesAsync();
            }

            // Update existing order items
            /*if (updateOrderItems.Count > 0)
            {
                unitOfWork.OrderItems.UpdateRange(updateOrderItems);
                await unitOfWork.SaveChangesAsync();
            }*/
        }

        private static Order OrderMapper(GHN_SearchOrderDataDto o, Guid? shopId = null, int deliveryFee = 0)
        {
            var entityOrder = new Order
            {
                ShopId = shopId,
                IsPublished = true,
                LastSyncDate = DateTime.UtcNow,
                DeliveryPartner = EnumSupplierConstants.GHN,

                PartnerShopId = $"{o.ShopId}",
                DeliveryFee = deliveryFee,
                Note = o.Note,
                ReturnName = o.ReturnName,
                ReturnPhone = o.ReturnPhone,
                ReturnAddress = o.ReturnAddress,
                ReturnWardCode = o.ReturnWardCode,
                ReturnDistrictId = $"{o.ReturnDistrictId}",
                FromName = o.FromName,
                FromPhone = o.FromPhone,
                FromAddress = o.FromAddress,
                FromWardId = o.FromWardCode,
                FromDistrictId = o.FromDistrictId,
                ToName = o.ToName,
                ToPhone = o.ToPhone,
                ToAddress = o.ToAddress,
                ToWardId = o.ToWardCode,
                ToDistrictId = o.ToDistrictId,

                Weight = o.Weight,
                Length = o.Length,
                Width = o.Width,
                Height = o.Height,
                ConvertedWeight = o.ConvertedWeight,
                CalculateWeight = o.CalculateWeight,
                ServiceTypeId = o.ServiceTypeId,
                ServiceId = o.ServiceId,
                PaymentTypeId = o.PaymentTypeId,
                PickStationId = o.PickStationId,
                DeliverStationId = o.DeliverStationId,
                Content = o.Content,
                InsuranceValue = o.InsuranceValue,
                Coupon = o.Coupon,
                PickShift = o.PickShift,
                CurrentStatus = o.Status,
                RequiredNote = o.RequiredNote,
                CodAmount = o.CodAmount,
                CodFailedAmount = o.CodFailedAmount,

                Created = DateTime.UtcNow
            };

            if (!string.IsNullOrWhiteSpace(o.ClientOrderCode))
            {
                entityOrder.UniqueCode = o.ClientOrderCode;
                entityOrder.ClientOrderCode = o.ClientOrderCode;
            }

            return entityOrder;
        }

        private static OrderItem OrderItemMapper(ItemDto i)
        {
            return new OrderItem
            {
                Name = i.Name,
                Code = i.Code,
                Quantity = i.Quantity,
                Length = i.Length,
                Width = i.Width,
                Height = i.Height,
                Status = i.Status,
                ItemOrderCode = i.ItemOrderCode
            };
        }
    }
}
