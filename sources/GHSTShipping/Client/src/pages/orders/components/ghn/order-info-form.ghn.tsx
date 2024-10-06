import React, { useState } from 'react';
import { Form, Input, Checkbox, InputNumber, Row, Col, Card } from 'antd';
import { InfoCircleOutlined } from '@ant-design/icons';
import MyInputNumber from '../MyInputNumber';

const OrderInfoForm = () => {
  const [failedDelivery, setFailedDelivery] = useState(false);

  const onFailedDeliveryChange = (e: any) => {
    setFailedDelivery(e.target.checked);
  };

  return (
    <Card title="Thông tin đơn hàng" style={{ marginBottom: '16px' }}>
      <Row gutter={16}>
        {/* Weight */}
        <Col span={6}>
          <Form.Item label="KL (gram)" name={'weight'}>
            <MyInputNumber defaultValue="500"/>
          </Form.Item>
        </Col>

        {/* Dimensions */}
        <Col span={4}>
          <Form.Item label="Dài (cm)" name={'length'}>
            <MyInputNumber defaultValue="10" max={999999}/>
          </Form.Item>
        </Col>
        <Col span={4}>
          <Form.Item label="Rộng (cm)" name={'width'}>
            <MyInputNumber defaultValue="10" max={999999}/>
          </Form.Item>
        </Col>
        <Col span={4}>
          <Form.Item label="Cao (cm)" name={'height'}>
            <MyInputNumber defaultValue="10" max={999999}/>
          </Form.Item>
        </Col>

        {/* Equivalent Weight */}
        <Col span={6}>
          <Form.Item label="KL quy đổi (gram)">
            <Input defaultValue="200" disabled />
          </Form.Item>
        </Col>
      </Row>

      <Row gutter={16}>
        {/* COD */}
        <Col span={12}>
          <Form.Item
            name="cod_amount"
            label="Tổng tiền thu hộ (COD)"
            tooltip={{ title: 'Nhập số tiền COD', icon: <InfoCircleOutlined /> }}
          >
            <MyInputNumber defaultValue="0" max={99999999}/>
          </Form.Item>
        </Col>

        {/* Total Value */}
        <Col span={12}>
          <Form.Item
            name="insurance_value"
            label="Tổng giá trị hàng hóa"
            tooltip={{ title: 'Nhập tổng giá trị hàng hóa', icon: <InfoCircleOutlined /> }}
          >
            <MyInputNumber defaultValue="0" max={99999999}/>
          </Form.Item>
        </Col>
      </Row>

      <Row gutter={16}>
        {/* Failed Delivery Checkbox */}
        <Col span={24}>
          <Checkbox checked={failedDelivery} onChange={onFailedDeliveryChange}>
            Giao thất bại thu tiền
          </Checkbox>
        </Col>

        {/* Amount for Failed Delivery */}
        {failedDelivery && (
          <Col span={12}>
            <Form.Item name="cod_failed_amount">
              <MyInputNumber defaultValue="20000" max={99999999}/>
            </Form.Item>
          </Col>
        )}
      </Row>
    </Card>
  );
};

export default OrderInfoForm;
