using GHSTShipping.Domain.Entities;
using System;

namespace GHSTShipping.Domain.DTOs
{
    public class OrderDto
    {
        public int No { get; set; }
        public bool IsPublished { get; set; }
        public DateTime? PublishDate { get; set; }

        public DateTime Created { get; set; }

        public Guid Id { get; set; }
        public Guid? ShopId { get; set; }
        public string ShopName { get; set; }
        public string DeliveryPartner { get; set; }
        public int DeliveryFee { get; set; }

        public string ClientOrderCode { get; set; }
        public string FromName { get; set; }
        public string FromPhone { get; set; }
        public string FromAddress { get; set; }
        public string FromWardName { get; set; }
        public string FromDistrictName { get; set; }
        public string FromProvinceName { get; set; }

        public string ToName { get; set; }
        public string ToPhone { get; set; }
        public string ToAddress { get; set; }
        public string ToWardName { get; set; }
        public string ToDistrictName { get; set; }
        public string ToProvinceName { get; set; }

        /// <summary>
        /// Input of shop
        /// </summary>
        public int RootWeight { get; set; }
        public int RootLength { get; set; }
        public int RootWidth { get; set; }
        public int RootHeight { get; set; }

        public int Weight { get; set; }
        public int Length { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public int CodAmount { get; set; }
        public int InsuranceValue { get; set; }

        /// <summary>
        /// 2: Hàng nhẹ, 5: Hàng nặng
        /// </summary>
        public int ServiceTypeId { get; set; }
        public string ServiceTypeName
        {
            get
            {
                if (ServiceTypeId == 2)
                {
                    return "Hàng nhẹ";
                }

                if (ServiceTypeId == 5)
                {
                    return "Hàng nặng";
                }

                return "";
            }
        }

        /// <summary>
        /// 1: Người bán/Người gửi. 2: Người mua/Người nhận.
        /// </summary>
        public int PaymentTypeId { get; set; }
        public string PaymentTypeName
        {
            get
            {
                if (PaymentTypeId == 1)
                {
                    return "Người gửi trả phí";
                }

                if (PaymentTypeId == 2)
                {
                    return "Người nhận trả phí";
                }

                return "";
            }
        }

        public string Status { get; set; }
        public string StatusName
        {
            get
            {
                if (Status == OrderStatus.DRAFT)
                {
                    return "Nháp";
                }

                if (Status == OrderStatus.WAITING_CONFIRM)
                {
                    return "Chờ xác nhận";
                }

                if (Status == OrderStatus.READY_TO_PICK)
                {
                    return "Chờ lấy hàng";
                }

                if (Status == OrderStatus.CANCEL)
                {
                    return "Đã hủy";
                }

                return OrderStatus.GetStatusTranslation(Status);
            }
        }

        public string StatusColor
        {
            get
            {
                if (Status == OrderStatus.DRAFT)
                {
                    return "gray";
                }

                if (Status == OrderStatus.WAITING_CONFIRM)
                {
                    return "gray";
                }

                if (Status == OrderStatus.READY_TO_PICK)
                {
                    return "orange";
                }

                if (Status == OrderStatus.CANCEL)
                {
                    return "red";
                }

                return OrderStatus.GetStatusColor(Status);
            }
        }

        public string PrivateOrderCode { get; set; }

        public string RootDisplaySize
        {
            get
            {
                string displaySize = $"{RootLength}x{RootWidth}x{RootHeight}";
                return displaySize;
            }
        }

        public int RootConvertedWeight
        {
            get
            {
                int convertedWeight = RootLength * RootWidth * RootHeight;
                return convertedWeight;
            }
        }

        public string DisplaySize
        {
            get
            {
                string displaySize = $"{Length}x{Width}x{Height}";
                return displaySize;
            }
        }

        public int ConvertedWeight
        {
            get
            {
                int convertedWeight = Length * Width * Height;
                return convertedWeight;
            }
        }
    }
}
