import { Col, Form, Input, Row, Switch, Typography } from 'antd';
import { IDeliveryConfigDto } from '.';

interface PartnerConfigProps {
  data: IDeliveryConfigDto;
  index: number;
}

const PartnerConfig = (props: PartnerConfigProps) => {
  const { data, index } = props;
  return (
    <Col span={12}>
      <Form.Item initialValue={data.id} hidden name={['deliveryConfigs', index, 'id']}></Form.Item>
      <Form.Item initialValue={data.apiKey} name={['deliveryConfigs', index, 'apiKey']} label="Api Key" required>
        <Input placeholder="Vui lòng nhập giá trị" />
      </Form.Item>
      <Form.Item initialValue={data.userName} name={['deliveryConfigs', index, 'userName']} label="Tên tài khoản">
        <Input placeholder="Vui lòng nhập giá trị" />
      </Form.Item>
      <Form.Item
        initialValue={data.isActivated}
        name={['deliveryConfigs', index, 'isActivated']}
        label="Bật / Tắt"
        valuePropName="checked"
      >
        <Switch checkedChildren="Đang bật" unCheckedChildren="Đang tắt" size="default" />
      </Form.Item>
    </Col>
  );
};

export default PartnerConfig;
