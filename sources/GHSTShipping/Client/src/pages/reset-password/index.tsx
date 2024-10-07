import React, { useState } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import { Form, Input, Button, message } from 'antd';
import { apiResetPassword } from '@/api/user.api';
import { IResetPasswordDto } from '@/interface/user/login';

const ResetPasswordPage: React.FC = () => {
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();

  // Trích xuất token và email từ URL
  const location = useLocation();
  const searchParams = new URLSearchParams(location.search);
  const token = searchParams.get('token') || "";
  const email = searchParams.get('email');

  const onFinish = async (values: IResetPasswordDto) => {
    if (values.password !== values.confirmPassword) {
      message.error('Mật khẩu không khớp!');
      return;
    }

    setLoading(true);
    try {
      const response = await apiResetPassword({
        ...values,
        token: token ?? '',
        email: email ?? '',
      });

      if (response.success) {
        message.success('Đặt lại mật khẩu thành công!');
        navigate('/login'); // Chuyển hướng đến trang đăng nhập
      } else {
        console.error(response);
        message.error('Link đặt lại mật khẩu đã hết hạn, vui lòng kiểm tra lại.');
      }
    } catch (error) {
      message.error('Đặt lại mật khẩu không thành công, vui lòng thử lại.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div style={{ maxWidth: 350, margin: 'auto', padding: '20px' }}>
      <h2 style={{ textAlign: 'center' }}>Đặt lại mật khẩu</h2>
      <Form<IResetPasswordDto> onFinish={onFinish} layout="vertical">
        <Form.Item
          name="password"
          label="Mật khẩu mới"
          rules={[{ required: true, message: 'Vui lòng nhập mật khẩu mới!' }]}
        >
          <Input.Password />
        </Form.Item>

        <Form.Item
          name="confirmPassword"
          label="Xác nhận mật khẩu"
          rules={[{ required: true, message: 'Vui lòng xác nhận mật khẩu mới!' }]}
        >
          <Input.Password />
        </Form.Item>

        <Form.Item>
          <Button type="primary" htmlType="submit" loading={loading} block>
            Đặt lại mật khẩu
          </Button>
        </Form.Item>
      </Form>
    </div>
  );
};

export default ResetPasswordPage;
