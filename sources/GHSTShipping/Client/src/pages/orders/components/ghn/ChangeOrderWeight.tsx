import React, { useEffect, useState } from 'react';
import { Card, Col, Form, InputNumber, Modal, Row } from 'antd';
import { IOrderViewDto } from '@/interface/order/order.interface';
import NumberFormatter from '@/components/core/NumberFormatter';
import { debounce } from '@/utils/common';
import { request } from '@/api/base/request';

interface ChangeOrderWeightProps {
  order?: IOrderViewDto;
  open: boolean;
  onSubmit?: (order: IOrderViewDto | undefined, values: { newLength: number; newWidth: number; newHeight: number }) => void;
  onCancel: () => void;
}

const ChangeOrderWeight: React.FC<ChangeOrderWeightProps> = ({ order, onSubmit, open, onCancel }) => {
  const [convertedWeight, setConvertedWeight] = useState<number>();
  const [calcWeight, setCalcWeight] = useState<number>();
  const [shippingCost, setShippingCost] = useState<number>();

  const [form] = Form.useForm();
  const handleFinish = (values: { newLength: number; newWidth: number; newHeight: number }) => {
    onSubmit && onSubmit(order, values);
  };

  const handleOk = () => {
    form.submit();
  };

  const handleCalc = debounce(() => {
    calApiCalc();
  }, 300);

  const calApiCalc = async () => {
    const values = form.getFieldsValue();
    console.log('values ', values);
    if (Boolean(order)) {
      const editOrder = order as IOrderViewDto;
      const payload = {
        ...values,
        shopDeliveryPricePlaneId: editOrder.orderDeiveryPricePlanDetail?.id ?? '',
      };
      const response = await request('post', '/orders/calculate-shipping-cost', payload);
      if (response.success) {
        const convertedWeight = response?.data?.calcOrderWeight ?? 0;
        const calcWeight = response?.data?.orderWeight ?? 0;
        const shippingCost = response?.data?.shippingCost ?? 0;
        setConvertedWeight(convertedWeight);
        setCalcWeight(calcWeight);
        setShippingCost(shippingCost);
      }
    }
  };

  useEffect(() => {
    if (Boolean(order) && Boolean(order?.id)) {
      form.resetFields();
      form.setFieldsValue({
        ...order,
      });

      const convertedWeight = order?.convertedWeight ?? 0;
      const calcWeight = order?.convertedWeight ?? 0;
      const deliveryFee = order?.customDeliveryFee ?? 0;
      setConvertedWeight(convertedWeight);
      setCalcWeight(calcWeight);
      setShippingCost(deliveryFee);
    }
  }, [order?.id]);

  return (
    <Modal centered title={`Đơn hàng #${order?.id}`} open={open} onOk={handleOk} onCancel={onCancel} destroyOnClose>
      <Card
        title="Thông tin khách hàng nhập"
        style={{ background: '#d4d4d4' }}
        styles={{
          body: {
            padding: 20,
            paddingTop: 0,
            paddingBottom: 0,
          },
        }}
      >
        <div>
          <p>
            <strong>Kích thước (DxRxC):</strong> {order?.rootLength} x {order?.rootWidth} x {order?.rootHeight} (cm)
          </p>
        </div>
        <div>
          <p>
            <strong>KL quy đổi:</strong> {<NumberFormatter value={order?.rootConvertedWeight as number} style="unit" unit="gram" />}
          </p>
        </div>
        <div>
          <p>
            <strong>KL tính phí:</strong> {<NumberFormatter value={order?.rootCalculateWeight as number} style="unit" unit="gram" />}
          </p>
        </div>
        <div>
          <p>
            <strong>Giá trị đơn hàng:</strong>{' '}
            {<NumberFormatter value={order?.insuranceValue as number} style="currency" currency="VND" locale="vi-VN" />}
          </p>
        </div>
        <div>
          <p>
            <strong>Phí vận chuyển:</strong> {<NumberFormatter value={order?.deliveryFee as number} style="currency" currency="VND" locale="vi-VN" />}
          </p>
        </div>
      </Card>
      <br />
      <Card
        title="Thông tin gửi sang đơn vị vận chuyển"
        styles={{
          body: {
            padding: 20,
            paddingTop: 0,
            paddingBottom: 0,
          },
        }}
      >
        <Form form={form} layout="vertical" onFinish={handleFinish} onFieldsChange={handleCalc}>
          <Row gutter={[8, 8]}>
            <Form.Item hidden label="Giá trị đơn hàng" name="insuranceValue" rules={[{ required: true, message: 'Vui lòng nhập' }]}>
              <InputNumber />
            </Form.Item>

            <Col span={8}>
              <Form.Item label="Chiều dài (cm)" name="length" rules={[{ required: true, message: 'Vui lòng nhập' }]}>
                <InputNumber min={1} placeholder="Nhập chiều dài" style={{ width: '100%' }} />
              </Form.Item>
            </Col>
            <Col span={8}>
              <Form.Item label="Chiều rộng (cm)" name="width" rules={[{ required: true, message: 'Vui lòng nhập' }]}>
                <InputNumber min={1} placeholder="Nhập chiều rộng" style={{ width: '100%' }} />
              </Form.Item>
            </Col>
            <Col span={8}>
              <Form.Item label="Chiều cao (cm)" name="height" rules={[{ required: true, message: 'Vui lòng nhập' }]}>
                <InputNumber min={1} placeholder="Nhập chiều cao" style={{ width: '100%' }} />
              </Form.Item>
            </Col>
            <Col span={8}>
              <Form.Item label="Cân nặng (gram)" name="weight" rules={[{ required: true, message: 'Vui lòng nhập' }]}>
                <InputNumber min={1} placeholder="Nhập giá trị" style={{ width: '100%' }} />
              </Form.Item>
            </Col>
          </Row>
          <div>
            KL chuyển đổi: <NumberFormatter value={convertedWeight as number} style="unit" unit="gram" />
          </div>
          <div>
            <b>
              KL tính phí: <NumberFormatter value={calcWeight as number} style="unit" unit="gram" />
            </b>
          </div>
          <div>
            <b>
              <span style={{ paddingRight: '5px' }}>Phí vận chuyển:</span>
              <NumberFormatter value={shippingCost as number} style="currency" currency="VND" locale="vi-VN" />
            </b>
          </div>
        </Form>
      </Card>
    </Modal>
  );
};

export default ChangeOrderWeight;
