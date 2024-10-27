import type { IDeliveryConfigDto } from '.';

import { Col, Form, Input, Row, Switch, Typography } from 'antd';

interface PartnerConfigProps {
  data: IDeliveryConfigDto | null;
  index?: number;
}

const PartnerConfig = (props: PartnerConfigProps) => {
  const { data, index } = props;
  return (
    <Col span={24}>
      <Form.Item initialValue={data?.id} hidden name={'id'}></Form.Item>
      <Form.Item initialValue={data?.apiKey} name={'apiKey'} label="Api Key" required>
        <Input placeholder="Vui lòng nhập giá trị" />
      </Form.Item>
      <Form.Item initialValue={data?.userName} name={'userName'} label="Tên tài khoản">
        <Input placeholder="Vui lòng nhập giá trị" />
      </Form.Item>

      <Form.Item label="Họ tên" name="fullName" rules={[{ required: true, message: 'Vui lòng nhập' }]}>
        <Input placeholder="Nhập họ tên" />
      </Form.Item>

      <Form.Item label="Email" name="email" rules={[{ required: true, message: 'Vui lòng nhập' }]}>
        <Input placeholder="Nhập email" />
      </Form.Item>

      <Form.Item label="Số điện thoại" name="phoneNumber" rules={[{ required: true, message: 'Vui lòng nhập' }]}>
        <Input placeholder="Nhập số điện thoại" />
      </Form.Item>

      <Form.Item initialValue={data?.isActivated} name={'isActivated'} label="Bật / Tắt" valuePropName="checked">
        <Switch checkedChildren="Đang bật" unCheckedChildren="Đang tắt" size="default" />
      </Form.Item>
    </Col>
  );
};

export default PartnerConfig;
