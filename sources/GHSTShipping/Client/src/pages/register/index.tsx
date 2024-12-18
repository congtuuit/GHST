import type { RuleObject } from 'antd/lib/form';
import type { StoreValue } from 'antd/lib/form/interface';
import { Button, Card, Col, Form, Input, message, Row, Select, Typography } from 'antd';
import React from 'react';
import { useNavigate } from 'react-router-dom';
import { apiRegisterShop } from '@/api/auth.api';
import VietNameBankSelector from './VietNameBankSelector';

export interface IRegisterFormValues {
  fullName: string;
  email: string;
  phoneNumber: string;
  shopName: string;
  avgMonthlyYield: string;
  password: string;
  confirmPassword: string;
  bankName: string;
  bankAccountNumber: string;
  bankAccountHolder: string;
  bankAddress: string;
}

const RegisterPage: React.FC = () => {
  const navigate = useNavigate();

  // Xử lý khi gửi form
  const onFinish = async (values: IRegisterFormValues) => {
    const response = await apiRegisterShop(values);

    if (response.success) {
      message.success('Đăng ký thành công');
      navigate('/login');
    } else {
      if (response.errors?.length > 0) {
        const error = response.errors[0];
        message.error(error.description);

        return;
      }
      message.error('Xảy ra lỗi, vui lòng kiểm tra lại');
    }
  };

  return (
    <Card>
      <Form<IRegisterFormValues> layout="vertical" onFinish={onFinish} style={{ maxWidth: '800px', margin: 'auto' }}>
        <Row gutter={[16, 16]}>
          {/* Phần bên trái */}
          <Col span={12}>
            <Form.Item label="Họ và Tên" name="fullName" rules={[{ required: true, message: 'Vui lòng nhập họ và tên của bạn' }]}>
              <Input placeholder="Nhập họ và tên" />
            </Form.Item>

            <Form.Item
              label="Email"
              name="email"
              rules={[
                { required: true, message: 'Vui lòng nhập địa chỉ email' },
                { type: 'email', message: 'Vui lòng nhập địa chỉ email hợp lệ' },
              ]}
            >
              <Input placeholder="Nhập email" />
            </Form.Item>

            <Form.Item label="Số điện thoại" name="phoneNumber" rules={[{ required: true, message: 'Vui lòng nhập số điện thoại' }]}>
              <Input placeholder="Nhập số điện thoại" />
            </Form.Item>

            <Form.Item label="Mật khẩu" name="password" rules={[{ required: true, message: 'Vui lòng nhập mật khẩu' }]} hasFeedback>
              <Input.Password placeholder="Nhập mật khẩu" />
            </Form.Item>

            <Form.Item
              label="Xác nhận mật khẩu"
              name="confirmPassword"
              dependencies={['password']}
              hasFeedback
              rules={[
                { required: true, message: 'Vui lòng xác nhận mật khẩu của bạn' },
                ({ getFieldValue }) => ({
                  validator(_: RuleObject, value: StoreValue) {
                    if (!value || getFieldValue('password') === value) {
                      return Promise.resolve();
                    }

                    return Promise.reject(new Error('Mật khẩu không khớp!'));
                  },
                }),
              ]}
            >
              <Input.Password placeholder="Xác nhận mật khẩu" />
            </Form.Item>
          </Col>

          {/* Phần bên phải */}
          <Col span={12}>
            <Form.Item label="Tên cửa hàng" name="shopName" rules={[{ required: true, message: 'Vui lòng nhập tên cửa hàng' }]}>
              <Input placeholder="Nhập tên cửa hàng" />
            </Form.Item>

            <Form.Item
              label="Qui mô vận chuyển"
              name="avgMonthlyYield"
              rules={[
                { required: true, message: 'Vui lòng chọn qui mô vận chuyển' },
              ]}
            >
              <Select defaultValue="Không có nhu cầu thường xuyên" options={[
                { value: 'Không có nhu cầu thường xuyên', label: 'Không có nhu cầu thường xuyên' },
                { value: 'Dưới 150 ĐH/tháng', label: 'Dưới 150 ĐH/tháng' },
                { value: 'Từ 150 - dưới 900 ĐH/tháng', label: 'Từ 150 - dưới 900 ĐH/tháng' },
                { value: 'Từ 900 - dưới 3.000 ĐH/tháng', label: 'Từ 900 - dưới 3.000 ĐH/tháng' },
                { value: 'Từ 3.000 - dưới 6.000 ĐH/tháng', label: 'Từ 3.000 - dưới 6.000 ĐH/tháng' },
                { value: 'Từ 6.000 ĐH/tháng trở lên', label: 'Từ 6.000 ĐH/tháng trở lên' },
              ]} />
            </Form.Item>

            <Form.Item label="Tên ngân hàng" name="bankName" rules={[{ required: true, message: 'Vui lòng nhập tên ngân hàng' }]}>
              <VietNameBankSelector />
            </Form.Item>

            <Form.Item
              label="Số tài khoản ngân hàng"
              name="bankAccountNumber"
              rules={[{ required: true, message: 'Vui lòng nhập số tài khoản ngân hàng' }]}
            >
              <Input placeholder="Nhập số tài khoản ngân hàng" />
            </Form.Item>

            <Form.Item label="Chủ tài khoản" name="bankAccountHolder" rules={[{ required: true, message: 'Vui lòng nhập tên chủ tài khoản' }]}>
              <Input placeholder="Nhập tên chủ tài khoản" />
            </Form.Item>

            <Form.Item label="Địa chỉ ngân hàng" name="bankAddress" rules={[{ required: true, message: 'Vui lòng nhập địa chỉ ngân hàng' }]}>
              <Input placeholder="Nhập địa chỉ ngân hàng" />
            </Form.Item>
          </Col>
        </Row>

        {/* Nút Đăng ký */}
        <Form.Item>
          <Button type="primary" htmlType="submit" block style={{ minHeight: '25px' }}>
            Đăng ký
          </Button>
        </Form.Item>

        <Row align="middle" justify="center">
          <Typography.Text style={{ margin: 'auto' }}>
            Bạn đã có tài khoản? <Typography.Link onClick={() => navigate('/login')}>Đăng nhập</Typography.Link>
          </Typography.Text>
        </Row>
      </Form>
    </Card>
  );
};

export default RegisterPage;
