import type { FC } from 'react';
import { useEffect, useState } from 'react';
import Overview from './overview';
import SalePercent from './salePercent';
import TimeLine from './timeLine';
import './index.less';

const AdminDashBoard: FC = () => {
  const [loading, setLoading] = useState(true);

  // mock timer to mimic dashboard data loading
  useEffect(() => {
    const timer = setTimeout(() => {
      setLoading(undefined as any);
    }, 1000);

    return () => {
      clearTimeout(timer);
    };
  }, []);

  return (
    <div>
      <Overview loading={loading} />
      <SalePercent loading={loading} />
      <TimeLine loading={loading} />
    </div>
  );
};

export default AdminDashBoard;
