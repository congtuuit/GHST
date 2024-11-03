import React from 'react';
import { Tag } from 'antd';

interface OrderStatusProps {
  isPublished?: boolean;
  status?: string;
  statusName?: string;
  statusColor?: string;
}

const OrderStatus: React.FC<OrderStatusProps> = ({ isPublished, status, statusName, statusColor }) => {
  const text = isPublished ? statusName : status === 'waiting_confirm' ? 'Chờ xác nhận' : statusName;
  const color = statusColor ?? 'gray';

  return <Tag color={color}>{text}</Tag>;
};

export default OrderStatus;
