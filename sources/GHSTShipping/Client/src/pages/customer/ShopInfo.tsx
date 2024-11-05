import type { IShopViewDetailDto } from '@/interface/shop';
import { Col, Descriptions, Row, Tag, Card, Switch } from 'antd';
import moment from 'moment';
import React, { useEffect, useState } from 'react';
import { IChangeOperationConfig } from '@/api/type';

interface ShopInfoProps {
  data: IShopViewDetailDto | undefined;
  onChange?: (obj: IChangeOperationConfig) => void;
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

  const handleChangeOperationConfig = (payload: IChangeOperationConfig) => {
    onChange && onChange(payload);
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
            <Descriptions.Item label="Mã Cửa Hàng: ">{detail?.shopUniqueCode}</Descriptions.Item>
            <Descriptions.Item label="Ngày Đăng Ký: ">
              {detail?.registerDate ? moment(detail?.registerDate).format('DD/MM/YYYY') : 'Không có'}
            </Descriptions.Item>
            <Descriptions.Item label="Tên Cửa Hàng: ">{detail?.shopName}</Descriptions.Item>
            <Descriptions.Item label="Chủ Sở Hữu: ">{detail?.fullName}</Descriptions.Item>
            <Descriptions.Item label="Email: ">{detail?.email}</Descriptions.Item>
            <Descriptions.Item label="Sản lượng đơn trung bình 1 tháng: ">
              {detail?.avgMonthlyCapacity?.toLocaleString() ?? 'Không có'}
            </Descriptions.Item>
            <Descriptions.Item label="Số Điện Thoại: ">{detail?.phoneNumber}</Descriptions.Item>
          </Descriptions>
        </Card>
      </Col>
      <Col span={12}>
        <Card type="inner" title="Tài khoản ngân hàng: " bordered={true} style={{ marginBottom: '20px' }}>
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
              <Switch
                title={'Cho phép'}
                onChange={value =>
                  handleChangeOperationConfig({
                    shopId: detail?.id as string,
                    allowPublishOrder: value,
                  })
                }
                checked={detail?.allowPublishOrder ?? false}
              />
            </Descriptions.Item>
            <span style={{ fontStyle: 'italic' }}>{'(cho phép shop tạo & đẩy đơn sang đơn vị vận chuyển)'}</span>
            <br />

            <Descriptions.Item label="Cho Phép Sử Dụng Địa Chỉ Mặc Định:">
              <Switch
                title={'Cho phép'}
                onChange={value =>
                  handleChangeOperationConfig({
                    shopId: detail?.id as string,
                    allowUsePartnerShopAddress: value,
                  })
                }
                checked={detail?.allowUsePartnerShopAddress ?? false}
              />
            </Descriptions.Item>
            <span style={{ fontStyle: 'italic' }}>{'(Sử dụng địa chỉ của cửa hàng trên tài khoản đơn vị vận chuyển để tạo đơn)'}</span>
            <br />

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
