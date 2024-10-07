import { lazy, type FC } from 'react';

import { Card, Row, Typography } from 'antd';

import { LocaleFormatter } from '@/locales';
import './index.css';

const { Title, Paragraph } = Typography;

const CustomerTable = lazy(() => import(/* webpackChunkName: "table'"*/ '@/pages/customer/customer-table'));

const CustomerPage: FC = () => {
  return (
    <Card>
      <CustomerTable />
    </Card>
  );
};

export default CustomerPage;
