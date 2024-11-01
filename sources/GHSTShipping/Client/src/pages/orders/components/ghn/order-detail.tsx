import { Button, Modal, Tag } from 'antd';
import React, { useEffect, useState } from 'react';
import { Card, Col, Row, Typography, Divider } from 'antd';
import { IOrderDetail } from '@/interface/order/order.interface';
import MyModal from '@/components/core/modal';
const { Title, Text } = Typography;

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
      <MyModal
        title={<div>{"Chi tiết đơn hàng"} {data?.clientOrderCode}</div>}
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
          <Col span={12}>
            <Title level={4}>Thông tin Người gửi</Title>
            <Text strong>Tên:</Text> <Text>{data?.fromName}</Text>
            <br />
            <Text strong>Số điện thoại:</Text> <Text>{data?.fromPhone}</Text>
            <br />
            <Text strong>Địa chỉ:</Text>
            <Text>{`${data?.fromAddress}, ${data?.fromWardName}, ${data?.fromDistrictName}, ${data?.fromProvinceName}`}</Text>
          </Col>

          {/* Thông tin Người nhận */}
          <Col span={12}>
            <Title level={4}>Thông tin Người nhận</Title>
            <Text strong>Tên:</Text> <Text>{data?.toName}</Text>
            <br />
            <Text strong>Số điện thoại:</Text> <Text>{data?.toPhone}</Text>
            <br />
            <Text strong>Địa chỉ:</Text>
            <Text>{`${data?.toAddress}, ${data?.toWardName}, ${data?.toDistrictName}, ${data?.toProvinceName}`}</Text>
          </Col>

          {/* Thông tin Vận chuyển và Thanh toán */}
          <Col span={12}>
            <Title level={4}>Vận chuyển và Thanh toán</Title>
            <Text strong>Đối tác vận chuyển:</Text> <Text>{data?.deliveryPartner}</Text>
            <br />
            <Text strong>Phí vận chuyển:</Text> <Text>{data?.deliveryFee}</Text>
            <br />
            <Text strong>COD:</Text> <Text>{data?.codAmount}</Text>
            <br />
            <Text strong>Giá trị bảo hiểm:</Text> <Text>{data?.insuranceValue}</Text>
            <br />
            <Text strong>Hình thức thanh toán:</Text> <Text>{data?.paymentTypeName}</Text>
          </Col>

          {/* Thông tin Đơn hàng */}
          <Col span={12}>
            <Title level={4}>Thông tin Đơn hàng</Title>
            <Text strong>ID Đơn hàng:</Text> <Text>{data?.clientOrderCode}</Text>
            <br />
            <Text strong>Xuất bản:</Text> <Text>{data?.isPublished ? 'Có' : 'Không'}</Text>
            <br />
            <Text strong>Ngày xuất bản:</Text> <Text>{data?.publishDate?.toLocaleString()}</Text>
            <br />
            <Text strong>Trạng thái:</Text>{' '}
            <Text>
              <Tag style={{ minWidth: '50px', textAlign: 'center' }} color={'volcano'}>
                {data?.statusName}
              </Tag>
            </Text>
          </Col>

          {/* Thông tin Kiện hàng */}
          <Col span={24}>
            <Divider />
            <Title level={4}>Thông tin Kiện hàng</Title>
            <Row>
              <Col span={6}>
                <Text strong>Trọng lượng:</Text> <Text>{data?.weight} kg</Text>
              </Col>
              <Col span={6}>
                <Text strong>Kích thước:</Text> <Text>{`${data?.length} x ${data?.width} x ${data?.height} cm`}</Text>
              </Col>
              <Col span={6}>
                <Text strong>Loại dịch vụ:</Text> <Text>{data?.serviceTypeName}</Text>
              </Col>
            </Row>
          </Col>
        </Row>
      </MyModal>
    </div>
  );
};

export default OrderDetailDialog;
