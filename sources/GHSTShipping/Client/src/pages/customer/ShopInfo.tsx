import type { IShopViewDetailDto } from '@/interface/shop';
import type { CheckboxChangeEvent } from 'antd/es/checkbox';
import { Button, Checkbox, Col, Descriptions, Modal, Row, Select, Tag, Card } from 'antd';
import moment from 'moment';
import React, { useEffect, useState } from 'react';
import { apiUpdateGhnShopId } from '@/api/business.api';

interface ShopInfoProps {
  data: IShopViewDetailDto | undefined;
  onChange?: (id: string) => void;
}

const ShopInfo: React.FC<ShopInfoProps> = ({ data, onChange }) => {
  const [detail, setDetail] = useState<IShopViewDetailDto | undefined>(data);
  const [open, setOpen] = useState(false);

  const showModal = () => {
    setOpen(true);
  };

  const handleClose = () => {
    setOpen(false);
  };

  const handleChangeAllowPublishOrder = (e: CheckboxChangeEvent) => {
    onChange && onChange(detail?.id as string);
  };

  useEffect(() => {
    if (Boolean(data)) {
      setDetail(data);
    } else {
      if (open === true) {
        handleClose();
      }
    }
  }, [data]);

  useEffect(() => {
    if (Boolean(detail)) {
      showModal();
    }
  }, [detail]);

  return (
    <Row gutter={[16, 16]}>
      <Col span={12}>
        <Card type="inner" title="Thông Tin Cửa Hàng" bordered={true} style={{ height: '385px' }}>
          <Descriptions
            column={1} // Adjust column to fit layout needs
            layout="horizontal"
            labelStyle={{ fontWeight: 'bold' }}
            contentStyle={{ textAlign: 'right' }}
            colon={false} // Remove colon for cleaner look
          >
            <Descriptions.Item label="Mã Cửa Hàng">{detail?.shopUniqueCode}</Descriptions.Item>
            <Descriptions.Item label="Ngày Đăng Ký">
              {detail?.registerDate ? moment(detail?.registerDate).format('DD/MM/YYYY') : 'Không có'}
            </Descriptions.Item>
            <Descriptions.Item label="Tên Cửa Hàng">{detail?.shopName}</Descriptions.Item>
            <Descriptions.Item label="Chủ Sở Hữu">{detail?.fullName}</Descriptions.Item>
            <Descriptions.Item label="Email">{detail?.email}</Descriptions.Item>
            <Descriptions.Item label="Sức Chứa Hàng Tháng">{detail?.avgMonthlyCapacity?.toLocaleString() ?? 'Không có'}</Descriptions.Item>
            <Descriptions.Item label="Số Điện Thoại">{detail?.phoneNumber}</Descriptions.Item>
          </Descriptions>
        </Card>
      </Col>
      <Col span={12}>
        <Card type="inner" title="Tài khoản ngân hàng" bordered={true} style={{ marginBottom: '20px' }}>
          <Descriptions
            column={1} // Adjust column to fit layout needs
            layout="horizontal"
            labelStyle={{ fontWeight: 'bold' }}
            contentStyle={{ textAlign: 'right' }}
            colon={false} // Remove colon for cleaner look
          >
            <Descriptions.Item label="Tên Ngân Hàng:">{detail?.bankName}</Descriptions.Item>
            <Descriptions.Item label="Số Tài Khoản:">{detail?.bankAccountNumber}</Descriptions.Item>
            <Descriptions.Item label="Chủ Tài Khoản Ngân Hàng:">{detail?.bankAccountHolder}</Descriptions.Item>
            <Descriptions.Item label="Địa Chỉ Ngân Hàng:">{detail?.bankAddress}</Descriptions.Item>
          </Descriptions>
        </Card>

        <Card type="inner" title="Cấu hình vận hành" bordered={true}>
          <Descriptions
            column={1}
            layout="horizontal"
            labelStyle={{ fontWeight: 'bold' }}
            contentStyle={{ textAlign: 'right' }}
            colon={false} // Remove colon for cleaner look
          >
            <Descriptions.Item label="Cho Phép Đăng Đơn Hàng:">
              <Checkbox onChange={handleChangeAllowPublishOrder} checked={detail?.allowPublishOrder ?? false}>
                Cho phép
              </Checkbox>
            </Descriptions.Item>
            <Descriptions.Item label="Trạng Thái:">
              <Tag color={detail?.isVerified ? 'green' : 'warning'}>{detail?.status}</Tag>
            </Descriptions.Item>
          </Descriptions>
        </Card>
      </Col>
    </Row>
  );
};

export default ShopInfo;
