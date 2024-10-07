import type { IForgotPasswordDto } from '@/interface/user/login';
import type { FC } from 'react';
import { Button, Card, Col, Form, Input, Row } from 'antd';
import { useDispatch } from 'react-redux';
import { useLocation, useNavigate } from 'react-router-dom';
import { formatSearch } from '@/utils/formatSearch';
import { apiForgotPassword } from '@/api/user.api';
import './index.less';

const ForgotPassword: FC = () => {
  const navigate = useNavigate();
  const location = useLocation();
  const dispatch = useDispatch();

  const onFinished = async (form: IForgotPasswordDto) => {
    const res = dispatch(await apiForgotPassword(form));
    console.log("res ", res);
    
    if (!!res) {
      const search = formatSearch(location.search);
      const from = search.from || { pathname: '/' };
      navigate(from);
    }
  };

  return (
    <Row className="login-wrapper">
      <Col className="right login-page" span={24} md={10} lg={8}>
        <Card className="card">
          <Form<IForgotPasswordDto> onFinish={onFinished} className="login-page-form">
            <h2 style={{ color: 'white' }}>ĐẶT LẠI MẬT KHẨU</h2>
            <Form.Item
              name="email"
              rules={[
                {
                  required: true,
                  message: 'Vui lòng nhập địa chỉ email',
                },
              ]}
            >
              <Input placeholder={'Email'} />
            </Form.Item>
            <Form.Item
              className="mb-0"
              name="phoneNumber"
              rules={[
                {
                  required: true,
                  message: 'Vui lòng nhập số điện thoại',
                },
              ]}
            >
              <Input placeholder={'Số điện thoại'} />
            </Form.Item>

            <Form.Item className="btn-submit mb-0">
              <Button htmlType="submit" type="primary" className="login-page-form_button">
                Cấp lại mật khẩu
              </Button>
            </Form.Item>

            <Button
              onClick={() => navigate('/login')}
              htmlType="button"
              className="login-page-form_button btn-forgot-password"
              type="ghost"
            >
              Đăng nhập
            </Button>
          </Form>
        </Card>
      </Col>
    </Row>
  );
};

export default ForgotPassword;
