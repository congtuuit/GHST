import { GHN_OrderPaymentType, GHN_OrderServiceType, GHN_OrderStatus } from '@/features/order/contants';
import { Form, Select } from 'antd';
import React, { CSSProperties, useState } from 'react';

interface OrderFilterProps {
  style?: CSSProperties;
  onFilterChange?: (filters: OrderFilterState) => void;
}

export interface OrderFilterState {
  status?: string;
  paymentTypeId?: number | null;
  isPrint?: boolean | null;
  isCodFailedCollected?: boolean;
  isDocumentPod?: boolean;
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
  const [form] = Form.useForm();
  const handleOnChangeForm = () => {
    const formValues: OrderFilterState = form.getFieldsValue();
    onFilterChange && onFilterChange(formValues);
  };

  const formItemStyle = {
    style: { marginBottom: '10px' },
    labelCol: { style: { paddingBottom: '0px' } },
  };

  return (
    <Form form={form} className="order-filter" layout="vertical" style={style} onFieldsChange={() => handleOnChangeForm()}>
      <Form.Item name="status" label="Trạng thái" {...formItemStyle}>
        <Select defaultValue="" options={OrderStatusOptions} />
      </Form.Item>

      <Form.Item name="paymentTypeId" label="Tùy chọn thanh toán" {...formItemStyle}>
        <Select defaultValue="" options={PaymentTypeOptions} />
      </Form.Item>

      <Form.Item name="isPrint" label="In đơn vận" {...formItemStyle}>
        <Select defaultValue="" options={OrderPrintedOptions} />
      </Form.Item>

      <Form.Item name="isCodFailedCollected" label="Giao thất bại - thu tiền" {...formItemStyle}>
        <Select defaultValue="" options={ReturnFailedOptions} />
      </Form.Item>

      <Form.Item name="isDocumentPod" label="Thu hồi chứng từ" {...formItemStyle}>
        <Select defaultValue="" options={ReturnDocsOptions} />
      </Form.Item>

      <Form.Item name="serviceTypeId" label="Loại đơn hàng" {...formItemStyle}>
        <Select defaultValue="" options={ServiceTypeOptions} />
      </Form.Item>
    </Form>
  );
};

export default OrderFilter;
