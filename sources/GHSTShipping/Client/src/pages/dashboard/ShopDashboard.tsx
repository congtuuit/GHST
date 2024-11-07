import type { FC } from 'react';
import { Button } from 'antd';
import { useNavigate } from 'react-router-dom';
import './index.less';

const ShopDashboard: FC = () => {
  const navigate = useNavigate();

  return (
    <div style={{ textAlign: 'center' }}>
      <div>
        <h1>Chào mừng bạn đến với GHST EXPRESS</h1>
        <div>
          <Button onClick={() => navigate('/order/list')} size="large">
            Xem tất cả đơn hàng
          </Button>
          <Button onClick={() => navigate('/order/create')} size="large" type="primary">
            Tạo đơn hàng
          </Button>
        </div>
      </div>
    </div>
  );
};

export default ShopDashboard;
