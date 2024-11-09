import React, { useEffect } from 'react';
import { Form, InputNumber, Modal } from 'antd';
import { IOrderViewDto } from '@/interface/order/order.interface';
import NumberFormatter from '@/components/core/NumberFormatter';

interface ChangeOrderWeightProps {
  order?: IOrderViewDto;
  open: boolean;
  onSubmit?: (order: IOrderViewDto | undefined, values: { newLength: number; newWidth: number; newHeight: number }) => void;
  onCancel: () => void;
}

const ChangeOrderWeight: React.FC<ChangeOrderWeightProps> = ({ order, onSubmit, open, onCancel }) => {
  const [form] = Form.useForm();
  const handleFinish = (values: { newLength: number; newWidth: number; newHeight: number }) => {
    onSubmit && onSubmit(order, values);
  };

  const handleOk = () => {
    form.submit();
  };

  useEffect(() => {
    form.resetFields();
  }, []);

  return (
    <Modal title={`Đơn hàng #${order?.id}`} open={open} onOk={handleOk} onCancel={onCancel} destroyOnClose>
      <p>
        <strong>Kích thước hiện tại:</strong> {order?.rootLength} x {order?.width} x {order?.height} (cm)
      </p>
      <p>
        <strong>Khối lượng quy đổi:</strong> {<NumberFormatter value={order?.convertedWeight as number} style="unit" unit="gram" />}
      </p>

      <Form form={form} layout="vertical" onFinish={handleFinish}>
        <Form.Item label="Chiều dài mới" name="length" rules={[{ required: true, message: 'Vui lòng nhập chiều dài mới' }]}>
          <InputNumber min={1} placeholder="Nhập chiều dài mới" style={{ width: '100%' }} />
        </Form.Item>

        <Form.Item label="Chiều rộng mới" name="width" rules={[{ required: true, message: 'Vui lòng nhập chiều rộng mới' }]}>
          <InputNumber min={1} placeholder="Nhập chiều rộng mới" style={{ width: '100%' }} />
        </Form.Item>

        <Form.Item label="Chiều cao mới" name="height" rules={[{ required: true, message: 'Vui lòng nhập chiều cao mới' }]}>
          <InputNumber min={1} placeholder="Nhập chiều cao mới" style={{ width: '100%' }} />
        </Form.Item>
      </Form>
    </Modal>
  );
};

export default ChangeOrderWeight;
