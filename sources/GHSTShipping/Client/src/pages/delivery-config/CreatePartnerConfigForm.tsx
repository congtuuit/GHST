import { Form, Input, Button, Select, message } from 'antd';
import { deliveryPartnerArray, EnumDeliveryPartner } from '@/constants/data';
import { apiCreateDeliveryConfig } from '@/api/business.api';

interface CreatePartnerConfigProps {
  onFinish: () => void;
}

const CreatePartnerConfigForm = (props: CreatePartnerConfigProps) => {
  const { onFinish } = props;
  const [form] = Form.useForm();

  const handleSubmit = async (values: any) => {
    const response = await apiCreateDeliveryConfig(values);
    if (response.success) {
      message.success('Thêm cấu hình thành công!');
      onFinish && onFinish();
    } else {
      message.error('Thao tác thất bại, vui lòng kiểm tra lại!');
    }
  };

  const onFinishFailed = (errorInfo: any) => {
    console.log('Failed:', errorInfo);
  };

  return (
    <Form
      form={form}
      name="create-partner-config"
      initialValues={{ remember: true }}
      onFinish={handleSubmit}
      onFinishFailed={onFinishFailed}
      autoComplete="off"
      layout="vertical"
    >
      <Form.Item label="Đơn vị vận chuyển" name="deliveryPartner" rules={[{ required: true, message: 'Vui lòng chọn đơn vị vận chuyển' }]}>
        <Select
          style={{ width: '100%' }} // Adjust width as needed
          placeholder="Chọn đơn vị vận chuyển"
        >
          {deliveryPartnerArray.map(i => (
            <Select.Option key={i.key} value={i.value} disabled={i.value !== EnumDeliveryPartner.GHN}>
              {i.key}
            </Select.Option>
          ))}
        </Select>
      </Form.Item>
      <Form.Item label="Khóa API" name="apiKey" rules={[{ required: true, message: 'Vui lòng nhập khóa API' }]}>
        <Input placeholder="Nhập khóa API" />
      </Form.Item>

      <Form.Item label="Tài khoản" name="userName">
        <Input placeholder="Nhập tên tài khoản" />
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

      <div style={{ marginTop: '40px', display: 'flex', justifyContent: 'right' }}>
        <Button type="default" htmlType="button" onClick={onFinish}>
          Hủy
        </Button>
        <Button type="primary" htmlType="submit">
          Thêm
        </Button>
      </div>
    </Form>
  );
};

export default CreatePartnerConfigForm;
