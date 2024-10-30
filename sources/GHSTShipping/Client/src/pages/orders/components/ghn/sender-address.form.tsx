import { Form, Input, Modal } from 'antd';
import { useEffect } from 'react';

import AddressComponent from '../address.component';

interface SenderAddressFormProps {
  open: boolean;
  onCancel: () => void;
  onSubmit: (values: ISenderAddress) => void;
}

export interface ISenderAddress {
  phone: string;
  name: string;
  address: string;
  district: string;
  districtName: string;
  ward: string;
  wardName: string;
  provinceName: string;
}

const SenderAddressForm = (props: SenderAddressFormProps) => {
  const { open, onCancel, onSubmit } = props;
  const [form] = Form.useForm();

  const handleSubmit = () => {
    form.validateFields().then(values => {
      onSubmit && onSubmit(values as ISenderAddress);
    });
  };

  const handleCancel = () => {
    onCancel && onCancel();
  };

  useEffect(() => {
    form.resetFields();
  }, [open]);

  return (
    <Modal maskClosable={false} title="Chỉnh sửa thông tin người gửi" open={open} onCancel={handleCancel} onOk={handleSubmit}>
      <Form form={form} layout="vertical">
        <Form.Item label="Số điện thoại" name="phone" rules={[{ required: true, message: 'Vui lòng nhập số điện thoại!' }]}>
          <Input placeholder="Nhập số điện thoại" />
        </Form.Item>

        <Form.Item label="Họ tên" name="name" rules={[{ required: true, message: 'Vui lòng nhập họ tên!' }]}>
          <Input placeholder="Nhập họ tên" />
        </Form.Item>
      </Form>
    </Modal>
  );
};

export default SenderAddressForm;
