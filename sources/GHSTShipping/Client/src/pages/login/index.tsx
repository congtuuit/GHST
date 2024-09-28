import type { LoginParams } from '@/interface/user/login';
import type { FC } from 'react';

import './index.less';

import { Button, Card, Checkbox, Col, Form, Input, Row } from 'antd';
import { useDispatch } from 'react-redux';
import { useLocation, useNavigate } from 'react-router-dom';

import { LocaleFormatter, useLocale } from '@/locales';
import { formatSearch } from '@/utils/formatSearch';

import { loginAsync } from '../../stores/user.action';

const initialValues: LoginParams = {
  username: 'congtuuit@gmail.com',
  password: 'aA@123123',
  // remember: true
};

const LoginForm: FC = () => {
  const navigate = useNavigate();
  const location = useLocation();
  const dispatch = useDispatch();
  const { formatMessage } = useLocale();

  const onFinished = async (form: LoginParams) => {
    const res = await dispatch(loginAsync(form));
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
          <Form<LoginParams> onFinish={onFinished} className="login-page-form" initialValues={initialValues}>
            <h2 style={{ color: 'white' }}>ĐĂNG NHẬP</h2>
            <Form.Item
              name="username"
              rules={[
                {
                  required: true,
                  message: 'Vui lòng nhập tài khoản',
                },
              ]}
            >
              <Input placeholder={'Nhập tài khoản'} />
            </Form.Item>
            <Form.Item
              className="mb-0"
              name="password"
              rules={[
                {
                  required: true,
                  message: 'Vui lòng nhập mật khẩu',
                },
              ]}
            >
              <Input type="password" placeholder={'Nhập mật khẩu'} />
            </Form.Item>
            <Button onClick={() => navigate('/forgot-password')} className="btn-forgot-password" type="link">
              Quên mật khẩu?
            </Button>
            <Form.Item className="btn-submit">
              <Button htmlType="submit" type="primary" className="login-page-form_button">
                Đăng nhập
              </Button>
            </Form.Item>
          </Form>
        </Card>
      </Col>
    </Row>
  );
};

export default LoginForm;
