import { GHN_OrderPaymentType, GHN_OrderServiceType, GHN_OrderStatus } from '@/features/order/contants';
import { Form, Select } from 'antd';
import React, { CSSProperties, useState } from 'react';

interface OrderFilterProps {
  style?: CSSProperties;
  onFilterChange?: (filters: OrderFilterState) => void;
}

interface OrderFilterState {
  status: string;
  startDate: string;
  endDate: string;
  customerName: string;
}

const OrderStatusOptions = [
  { value: '', label: 'Tất cả' },
  { value: GHN_OrderStatus.Cancel, label: 'Đơn hủy' },
];
const PaymentTypeOptions = [
  { value: '', label: 'Tất cả' },
  { value: GHN_OrderPaymentType.BenGuiTraPhi, label: 'Bên gửi trả phí' },
  { value: GHN_OrderPaymentType.BenNhanTraPhi, label: 'Bên nhận trả phí' },
];
const OrderPrintedOptions = [
  { value: '', label: 'Tất cả' },
  { value: true, label: 'Đã in đơn vận' },
  { value: false, label: 'Chưa in đơn vận' },
];
const ReturnFailedOptions = [
  { value: '', label: 'Tất cả' },
  { value: true, label: 'Có thu được tiền' },
  { value: false, label: 'Không thu được tiền' },
];

const ReturnDocsOptions = [
  { value: '', label: 'Tất cả' },
  { value: true, label: 'Đã thu hồi được chứng từ' },
  { value: false, label: 'Chưa thu hồi hoặc không sử dụng dịch vụ' },
];
const ServiceTypeOptions = [
  { value: '', label: 'Tất cả' },
  { value: GHN_OrderServiceType.HangNhe, label: 'Đơn hàng nhẹ' },
  { value: GHN_OrderServiceType.HangNang, label: 'Đơn hàng nặng' },
];

const OrderFilter: React.FC<OrderFilterProps> = ({ style, onFilterChange }) => {
  const [filters, setFilters] = useState<OrderFilterState>({
    status: '',
    startDate: '',
    endDate: '',
    customerName: '',
  });

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    const { name, value } = e.target;
    const updatedFilters = { ...filters, [name]: value };
    setFilters(updatedFilters);
    onFilterChange && onFilterChange(updatedFilters);
  };

  const formItemStyle = {
    style: { marginBottom: '10px' },
    labelCol: { style: { paddingBottom: '0px' } },
  };

  return (
    <Form className="order-filter" layout="vertical" style={style}>
      <Form.Item name="orderStatus" label="Trạng thái" {...formItemStyle}>
        <Select defaultValue="" options={OrderStatusOptions} />
      </Form.Item>

      <Form.Item name="orderPaymentType" label="Tùy chọn thanh toán" {...formItemStyle}>
        <Select defaultValue="" options={PaymentTypeOptions} />
      </Form.Item>

      <Form.Item name="isDeliveryReceiptPrint" label="In đơn vận" {...formItemStyle}>
        <Select defaultValue="" options={OrderPrintedOptions} />
      </Form.Item>

      <Form.Item name="isGotReturnAmount" label="Giao thất bại - thu tiền" {...formItemStyle}>
        <Select defaultValue="" options={ReturnFailedOptions} />
      </Form.Item>

      <Form.Item name="isRetrieveDocuments" label="Thu hồi chứng từ" {...formItemStyle}>
        <Select defaultValue="" options={ReturnDocsOptions} />
      </Form.Item>

      <Form.Item name="serviceTypeId" label="Loại đơn hàng" {...formItemStyle}>
        <Select defaultValue="" options={ServiceTypeOptions} />
      </Form.Item>
    </Form>
  );
};

export default OrderFilter;
