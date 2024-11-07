import { Button, Modal, Tag } from 'antd';
import React, { useEffect, useState } from 'react';
import { Card, Col, Row, Typography, Divider } from 'antd';
import { IOrderDetail } from '@/interface/order/order.interface';
import MyModal from '@/components/core/modal';
import Price from '@/components/core/price';
import OrderStatus from '../OrderStatus';
const { Title, Text } = Typography;

interface OrderDialogProps {
  data: IOrderDetail | undefined;
  showSenderAddress?: boolean;
}

const OrderDetailDialog: React.FC<OrderDialogProps> = ({ data, showSenderAddress = false }) => {
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

  const displayAddress = (address?: string, wardName?: string, disctrictName?: string, provinceName?: string) => {
    let fullAddress = address ?? "";
    if(Boolean(wardName)) {
      fullAddress += ", " + wardName;
    }
    if(Boolean(disctrictName)) {
      fullAddress += ", " + disctrictName;
    }
    if(Boolean(provinceName)) {
      fullAddress += ", " + provinceName;
    }

    return fullAddress;
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
      <MyModal
        width={1200}
        title={
          <div>
            {'Chi tiết đơn hàng'} {data?.clientOrderCode}
          </div>
        }
        open={isVisible}
        onCancel={handleClose}
        onClose={handleClose}
        footer={[
          <Button type="primary" key="back" onClick={handleClose}>
            Đóng
          </Button>,
        ]}
      >
        <Row gutter={[16, 24]}>
          {/* Thông tin Người gửi */}
          {showSenderAddress && (
            <Col span={12}>
              <Title level={4}>Thông tin Người gửi</Title>
              <Text strong>Tên: </Text> <Text>{data?.fromName}</Text>
              <br />
              <Text strong>Số điện thoại: </Text> <Text>{data?.fromPhone}</Text>
              <br />
              <Text strong>Địa chỉ: </Text>
              <Text>{`${displayAddress(data?.fromAddress, data?.fromWardName, data?.fromDistrictName, data?.fromProvinceName)}`}</Text>
            </Col>
          )}

          {/* Thông tin Người nhận */}
          <Col span={12}>
            <Title level={4}>Thông tin Người nhận</Title>
            <Text strong>Tên: </Text> <Text>{data?.toName}</Text>
            <br />
            <Text strong>Số điện thoại: </Text> <Text>{data?.toPhone}</Text>
            <br />
            <Text strong>Địa chỉ: </Text>
            <Text>{`${displayAddress(data?.toAddress, data?.toWardName, data?.toDistrictName, data?.toProvinceName)}`}</Text>
          </Col>

          {/* Thông tin Vận chuyển và Thanh toán */}
          <Col span={12}>
            <Title level={4}>Vận chuyển và Thanh toán</Title>
            <Text strong>Phí vận chuyển: </Text>
            <Text>
              <b>
                <Price value={data?.deliveryFee ?? 0} type="warning" />
              </b>
            </Text>
            <br />
            <Text strong>COD: </Text>
            <Text>
              <Price value={data?.codAmount ?? 0} type="success" />
            </Text>
            <br />
            <Text strong>Giá trị đơn hàng: </Text>
            <Text>
              <Price value={data?.insuranceValue ?? 0} type="success" />
            </Text>
            <br />
            <Text strong>Hình thức thanh toán:</Text> <Text>{data?.paymentTypeName}</Text>
          </Col>

          {/* Thông tin Đơn hàng */}
          <Col span={12}>
            <Title level={4}>Thông tin Đơn hàng</Title>
            <Text strong>Mã đơn hàng: </Text> <Text>{data?.clientOrderCode}</Text>
            <br />
            <Text strong>Trạng thái: </Text>
            <Text>
              <OrderStatus isPublished={data?.isPublished} status={data?.status} statusName={data?.statusName} statusColor={data?.statusColor} />
            </Text>
          </Col>

          {/* Thông tin Kiện hàng */}
          <Col span={24}>
            <Divider />
            <Title level={4}>Thông tin Kiện hàng</Title>
            <Row>
              <Col span={6}>
                <Text strong>Trọng lượng: </Text> <Text>{data?.weight} kg</Text>
              </Col>
              <Col span={6}>
                <Text strong>Kích thước: </Text> <Text>{`${data?.length} x ${data?.width} x ${data?.height} cm`}</Text>
              </Col>
              <Col span={6}>
                <Text strong>Loại dịch vụ: </Text> <Text>{data?.serviceTypeName}</Text>
              </Col>
            </Row>
          </Col>
        </Row>
      </MyModal>
    </div>
  );
};

export default OrderDetailDialog;
