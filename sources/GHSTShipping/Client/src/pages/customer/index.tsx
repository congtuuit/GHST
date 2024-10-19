import './index.css';
import { Card } from 'antd';
import { lazy } from 'react';
const CustomerTable = lazy(() => import(/* webpackChunkName: "table'"*/ '@/pages/customer/customer-table'));
const CustomerPage = () => {
  return (
    <Card>
      <CustomerTable />
    </Card>
  );
};

export default CustomerPage;
