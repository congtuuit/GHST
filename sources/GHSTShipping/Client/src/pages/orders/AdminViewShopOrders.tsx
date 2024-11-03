import React from 'react';
import ShopOrders from './components/ShopOrder';
import { useParams } from 'react-router-dom';

const AdminViewShopOrders: React.FC = () => {
  const { shopId } = useParams<{ shopId: string }>(); // Use a generic type to specify the type of params
  return <ShopOrders shopId={shopId} />;
};

export default AdminViewShopOrders;
