import type { IOrderDetail } from '@/interface/order/order.interface';

import { Button, Modal } from 'antd';
import { debug } from 'console';
import React, { useEffect, useState } from 'react';

interface OrderDialogProps {
  data: IOrderDetail | undefined;
}

const OrderDetailDialog: React.FC<OrderDialogProps> = ({ data }) => {
  const [orderDetail, setOrderDetail] = useState<IOrderDetail | undefined>();
  const [isVisible, setIsVisible] = useState(false); // To handle modal visibility

  // Function to open the dialog
  const showModal = () => {
    setIsVisible(true);
  };

  // Function to close the dialog
  const handleClose = () => {
    setIsVisible(false);
    setOrderDetail(undefined);
  };

  useEffect(() => {
    if (Boolean(data)) {
      setOrderDetail(data);
    } else {
      if (isVisible == true) {
        handleClose();
      }
    }
  }, [data]);

  useEffect(() => {
    if (Boolean(orderDetail)) {
      showModal();
    }
  }, [orderDetail]);

  return (
    <div>
      {/* Antd Modal/Dialog */}
      <Modal
        title="Chi tiết đơn hàng"
        open={isVisible}
        onCancel={handleClose}
        footer={[
          <Button key="back" onClick={handleClose}>
            Đóng
          </Button>,
        ]}
      >
        <p>
          <strong>Order ID:</strong> {orderDetail?.id}
        </p>
        <p>
          <strong>Order Code:</strong> {orderDetail?.clientOrderCode}
        </p>
        <p>
          <strong>Private Order Code:</strong> {orderDetail?.privateOrderCode}
        </p>
        <p>
          <strong>Status:</strong> {orderDetail?.status}
        </p>
      </Modal>
    </div>
  );
};

export default OrderDetailDialog;
