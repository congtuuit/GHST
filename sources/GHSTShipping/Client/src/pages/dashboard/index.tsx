import type { FC } from 'react';
import { useSelector } from 'react-redux';
import ShopDashboard from './ShopDashboard';
import AdminDashBoard from './AdminDashBoard';
import './index.less';

const DashBoardPage: FC = () => {
  const { session } = useSelector(state => state.user);
  const isAdmin = session.roles.includes('ADMIN');
  if (isAdmin) {
    return <AdminDashBoard />;
  } else {
    return <ShopDashboard />;
  }
};

export default DashBoardPage;
