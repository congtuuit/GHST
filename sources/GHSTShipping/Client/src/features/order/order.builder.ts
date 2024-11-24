import { IOrder } from './type';

class OrderBuilder {
  errors: { field: string; message: string }[];
  order: IOrder;

  constructor(order: IOrder) {
    this.errors = [];
    this.order = order;
  }

  validateOrder(): { verified: boolean; errors: { field: string; message: string }[] } {
    // Kiểm tra thông tin người nhận
    if (!this.order.to_name) {
      this.errors.push({ field: 'to_name', message: 'Thiếu thông tin người nhận.' });
    }
    if (!this.order.to_phone) {
      this.errors.push({ field: 'to_phone', message: 'Thiếu thông tin SĐT người nhận.' });
    }
    if (!this.order.to_address) {
      this.errors.push({ field: 'to_address', message: 'Vui lòng nhập địa chỉ' });
    }
    if (!this.order.to_ward_name) {
      this.errors.push({ field: 'to_ward_name', message: 'Vui lòng nhập phường/xã' });
    }
    if (!this.order.to_district_name) {
      this.errors.push({ field: 'to_district_name', message: 'Vui lòng nhập quận/huyện' });
    }

    // Kiểm tra khối lượng và kích thước
    if (this.order.weight > 50000) {
      this.errors.push({ field: 'weight', message: 'Khối lượng vượt quá giới hạn 50000 gram.' });
    }
    if (this.order.length > 200) {
      this.errors.push({ field: 'length', message: 'Chiều dài vượt quá giới hạn 200 cm.' });
    }
    if (this.order.width > 200) {
      this.errors.push({ field: 'width', message: 'Chiều rộng vượt quá giới hạn 200 cm.' });
    }
    if (this.order.height > 200) {
      this.errors.push({ field: 'height', message: 'Chiều cao vượt quá giới hạn 200 cm.' });
    }

    // Kiểm tra số tiền thu hộ
    if (this.order.cod_amount > 50000000) {
      this.errors.push({ field: 'cod_amount', message: 'Số tiền thu hộ vượt quá giới hạn 50.000.000' });
    }

    // Kiểm tra loại dịch vụ và người thanh toán
    if (!this.order.service_type_id) {
      this.errors.push({ field: 'service_type_id', message: 'Vui lòng chọn loại dịch vụ' });
    }
    if (!this.order.payment_type_id) {
      this.errors.push({ field: 'payment_type_id', message: 'Vui lòng chọn hình thức thanh toán' });
    }

    // Kiểm tra yêu cầu bắt buộc
    if (!this.order.required_note) {
      this.errors.push({ field: 'required_note', message: 'Thiếu thông tin required_note.' });
    }

    // Kiểm tra danh sách hàng hóa
    if (!this.order.items || this.order.items.length === 0) {
      this.errors.push({ field: 'items', message: 'Danh sách hàng hóa trống.' });
    } else {
      this.order.items.forEach((item: any, index: number) => {
        if (!item.name) {
          this.errors.push({ field: `items[${index}].name`, message: `Thiếu tên tại vị trí ${index}.` });
        }
        if (!item.quantity || item.quantity <= 0) {
          this.errors.push({ field: `items[${index}].quantity`, message: `Số lượng tại vị trí ${index} không hợp lệ.` });
        }
      });
    }

    // Trả về kết quả
    return {
      verified: this.errors.length === 0,
      errors: this.errors,
    };
  }

}

export default OrderBuilder;
