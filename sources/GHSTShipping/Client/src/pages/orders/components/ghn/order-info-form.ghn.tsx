import { InfoCircleOutlined } from '@ant-design/icons';
import { Card, Checkbox, Col, Form, Input, InputNumber, Row } from 'antd';
import React, { useEffect, useState } from 'react';

import MyInputNumber from '../MyInputNumber';
import { ServiceType, ServiceTypeValue } from './ServiceType';
import NumberFormatter from '@/components/core/NumberFormatter';

interface OrderInfoFormProps {
  convertedWeight: number;
  highlight: boolean;
  allowFailedDelivery?: boolean;
  serviceType: ServiceTypeValue;
}
const OrderInfoForm = (props: OrderInfoFormProps) => {
  const { convertedWeight, highlight, allowFailedDelivery, serviceType } = props;
  const [failedDelivery, setFailedDelivery] = useState(false);

  const onFailedDeliveryChange = (e: any) => {
    setFailedDelivery(e.target.checked);
  };

  useEffect(() => {
    if (allowFailedDelivery) {
      setFailedDelivery(allowFailedDelivery);
    }
  }, [allowFailedDelivery]);

  return (
    <Card title="Thông tin đơn hàng" style={{ marginBottom: '16px' }} className="custom-card">
      <Row gutter={16}>
        {/* Weight */}
        <Col span={4}>
          <Form.Item
            label="KL (gram)"
            name={'weight'}
            rules={[
              { required: true, message: 'Vui lòng nhập' },
              {
                type: 'number',
                min: 1,
                max: serviceType === ServiceType.HangNhe ? 50000 : 500000,
                message: serviceType === ServiceType.HangNhe ? 'Khối lượng từ 1 - 50.000 gram' : 'Khối lượng từ 1 - 500.000 gram',
                transform: value => {
                  return Number(value) || 0;
                },
              },
            ]}
          >
            <MyInputNumber
              disabled={serviceType === ServiceType.HangNang}
              style={{ color: `${!highlight ? 'orange' : 'black'}` }}
              max={serviceType === ServiceType.HangNhe ? 50000 : 500000}
              placeholder="Nhập giá trị"
            />
          </Form.Item>
        </Col>

        {/* Dimensions */}
        <Col span={4}>
          <Form.Item
            label="Dài (cm)"
            name={'length'}
            rules={[
              { required: true, message: 'Vui lòng nhập' },
              {
                type: 'number',
                min: 1,
                max: 50000,
                message: 'Chiều dài từ 1 - 200 cm',
                transform: value => {
                  return Number(value) || 0;
                },
              },
            ]}
          >
            <MyInputNumber disabled={serviceType === ServiceType.HangNang} max={200} placeholder="Nhập giá trị" />
          </Form.Item>
        </Col>
        <Col span={4}>
          <Form.Item
            label="Rộng (cm)"
            name={'width'}
            rules={[
              { required: true, message: 'Vui lòng nhập' },
              {
                type: 'number',
                min: 1,
                max: 50000,
                message: 'Chiều rộng từ 1 - 200 cm',
                transform: value => {
                  return Number(value) || 0;
                },
              },
            ]}
          >
            <MyInputNumber disabled={serviceType === ServiceType.HangNang} max={200} placeholder="Nhập giá trị" />
          </Form.Item>
        </Col>
        <Col span={4}>
          <Form.Item
            label="Cao (cm)"
            name={'height'}
            rules={[
              { required: true, message: 'Vui lòng nhập' },
              {
                type: 'number',
                min: 1,
                max: 50000,
                message: 'Chiều cao từ 1 - 200 cm',
                transform: value => {
                  return Number(value) || 0;
                },
              },
            ]}
          >
            <MyInputNumber disabled={serviceType === ServiceType.HangNang} max={200} placeholder="Nhập giá trị" />
          </Form.Item>
        </Col>
        <Col span={8}>
          <span>KL quy đổi (gram): </span>
          <span style={{ color: `${highlight ? 'orange' : 'black'}` }}>
            <NumberFormatter value={convertedWeight} style="decimal" />
          </span>
        </Col>
      </Row>

      <Row gutter={16}>
        {/* COD */}
        <Col span={12}>
          <Form.Item name="cod_amount" label="Tổng tiền thu hộ (COD)" tooltip={{ title: 'Nhập số tiền COD', icon: <InfoCircleOutlined /> }}>
            <MyInputNumber defaultValue="0" max={50000000} placeholder="Nhập giá trị" />
          </Form.Item>
        </Col>

        {/* Total Value */}
        <Col span={12}>
          <Form.Item
            name="insurance_value"
            label="Tổng giá trị hàng hóa"
            tooltip={{ title: 'Nhập tổng giá trị hàng hóa', icon: <InfoCircleOutlined /> }}
          >
            <MyInputNumber defaultValue="0" max={5000000} placeholder="Nhập giá trị" />
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
              <MyInputNumber defaultValue="" max={99999999} placeholder="Nhập giá trị" />
            </Form.Item>
          </Col>
        )}
      </Row>
    </Card>
  );
};

export default OrderInfoForm;
