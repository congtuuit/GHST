import type { IOrderStatus } from './orderStatus';
import type { IOrderPagedParameter, IPaginationResponse } from '@/interface/business';
import type { IOrderDetail, IOrderDto } from '@/interface/order/order.interface';
import type { RadioChangeEvent, TablePaginationConfig } from 'antd';
import type { FilterValue } from 'antd/es/table/interface';
import type { ColumnsType } from 'antd/lib/table';
import { SearchOutlined } from '@ant-design/icons';
import { Button, Card, Col, message, Radio, Row, Select, Tag } from 'antd';
import { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import { apiCancelOrderGhn, apiGetOrderDetail, apiGetOrders } from '@/api/business.api';
import Datatable from '@/components/core/datatable';
import Price from '@/components/core/price';
import { suppliers } from '@/constants/data';
import { commingSoon } from '@/utils/common';
import OrderDetailDialog from './components/ghn/order-detail';
import { orderStatuses } from './orderStatus';
import AdminOrderList from './AdminOrderList';
import ShopOrderList from './ShopOrderList';

const { Option } = Select;

const OrdersPage = () => {
  const { session } = useSelector(state => state.user);
  const isAdmin = session.roles.includes('ADMIN');

  const Component = isAdmin ? AdminOrderList : ShopOrderList;

  return <Component />;
};

export default OrdersPage;
