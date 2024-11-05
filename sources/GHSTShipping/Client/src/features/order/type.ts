import { Dayjs } from 'dayjs';

export interface IOrderItem {
  name: string; // Tên sản phẩm | Bắt buộc
  code: string; // Mã sản phẩm
  quantity: number; // Số lượng | Bắt buộc
  price: number; // Giá
  length: number; // Chiều dài
  width: number; // Chiều rộng
  height: number; // Chiều cao
  category: string | null; // Danh mục sản phẩm
}

export interface IOrder {
  from_name?: string; // Tên người gửi (có thể lấy từ ShopID nếu không cung cấp)
  from_phone: string; // Số điện thoại người gửi
  from_address: string; // Địa chỉ người gửi
  from_ward_name: string; // Phường người gửi
  from_district_name: string; // Quận/Huyện người gửi
  from_province_name: string; // Tỉnh/Thành phố người gửi

  to_name: string; // Tên người nhận | Bắt buộc
  to_phone: string; // Số điện thoại người nhận | Bắt buộc
  to_address: string; // Địa chỉ người nhận | Bắt buộc
  to_ward_name: string; // Phường người nhận | Bắt buộc
  to_district_name: string; // Quận/Huyện người nhận | Bắt buộc
  to_province_name: string; // Tỉnh/Thành phố người nhận

  return_phone?: string; // Số điện thoại trả hàng
  return_address?: string; // Địa chỉ trả hàng
  return_district_name?: string; // Quận/Huyện trả hàng
  return_ward_name?: string; // Phường trả hàng

  client_order_code?: string; // Mã đơn hàng của khách hàng (hệ thống tự sinh)
  cod_amount: number; // Số tiền thu hộ (Tối đa 50000000)
  content?: string; // Nội dung đơn hàng (tối đa 2000 ký tự)

  cod_failed_amount?: number; // Tiền thu thêm khi giao hàng thất bại
  weight: number; // Khối lượng đơn hàng (gram) | Bắt buộc
  length: number; // Chiều dài (cm) | Bắt buộc
  width: number; // Chiều rộng (cm) | Bắt buộc
  height: number; // Chiều cao (cm) | Bắt buộc

  pick_station_id?: number | null; // Mã trạm lấy hàng
  insurance_value?: number; // Giá trị bảo hiểm đơn hàng (Tối đa 5000000)
  coupon?: string; // Mã giảm giá

  service_id?: number; // Mã dịch vụ
  service_type_id: number; // Mã loại dịch vụ | Bắt buộc
  payment_type_id: number; // Mã người thanh toán | Bắt buộc

  note?: string; // Ghi chú cho tài xế (5000 ký tự)
  required_note: string; // Yêu cầu bắt buộc (CHOTHUHANG | CHOXEMHANGKHONGTHU | KHONGCHOXEMHANG)

  pick_shift?: any[]; // Mảng ca lấy hàng
  pickup_time?: number; // Thời gian lấy hàng (Unix timestamp)

  items: IOrderItem[]; // Danh sách sản phẩm | Bắt buộc
}

export interface IOrderFilter {
  fromDate: Dayjs | null;
  toDate: Dayjs | null;
  orderStatus?: string;
  orderPaymentType?: number;
  isDeliveryReceiptPrint?: boolean;
  isGotReturnAmount?: boolean;
  isRetrieveDocuments?: boolean;
  serviceTypeId?: number;
  groupStatus?:
    | 'nhap'
    | 'cho_ban_giao'
    | 'da_ban_giao_dang_giao'
    | 'da_ban_giao_dang_hoang'
    | 'cho_xac_nhan_giao_lai'
    | 'hoan_tat'
    | 'da_huy'
    | 'that_lac_hong';
}
