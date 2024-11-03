import type { FC } from 'react';

import './index.less';

import { useEffect, useState } from 'react';

import Overview from './overview';
import SalePercent from './salePercent';
import TimeLine from './timeLine';
import { Button, Card } from 'antd';
import { useNavigate } from 'react-router-dom';

const DashBoardPage: FC = () => {
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();

  // mock timer to mimic dashboard data loading
  useEffect(() => {
    const timer = setTimeout(() => {
      setLoading(undefined as any);
    }, 2000);

    return () => {
      clearTimeout(timer);
    };
  }, []);

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

export default DashBoardPage;
