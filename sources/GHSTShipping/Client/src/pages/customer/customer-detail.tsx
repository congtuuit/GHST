import type { IShopViewDetailDto } from '@/interface/shop';
import { Button, Modal, Tabs } from 'antd';
import React, { useEffect, useState } from 'react';
import ShopInfo from './ShopInfo';
import { InfoCircleOutlined, SettingOutlined } from '@ant-design/icons';
import ShopConfig from './ShopConfig';
import { IChangeOperationConfig } from '@/api/type';
import ShopDeliveryPricePlane from '../customer-price/ShopDeliveryPricePlane';

interface CustomerDetailProps {
  data: IShopViewDetailDto | undefined;
  onChange: (id: IChangeOperationConfig) => void;
  callback?: () => void;
}

const CustomerDetail: React.FC<CustomerDetailProps> = ({ data, onChange, callback }) => {
  const [detail, setDetail] = useState<IShopViewDetailDto | undefined>(data);
  const [open, setOpen] = useState(false);
  const [totalConnected, setTotalConnected] = useState<number>(0);
  const [activeTab, setActiveTab] = useState<string>('SHOP_INFO');
  const [partnerConfigId, setPartnerConfigId] = useState('');

  const showModal = () => {
    setActiveTab('SHOP_INFO');
    setOpen(true);
  };

  const handleClose = () => {
    setOpen(false);
  };

  const handleTabChange = (tabKey: string) => {
    setActiveTab(tabKey);
  };

  useEffect(() => {
    if (Boolean(data)) {
      setDetail(data);
    } else {
      if (open == true) {
        handleClose();
      }
    }
  }, [data]);

  useEffect(() => {
    if (Boolean(detail)) {
      showModal();
      setTotalConnected(detail?.shopConfigs?.length ?? 0);
      if (detail?.shopConfigs && detail?.shopConfigs.length > 0) {
        const _partnerConfigId = detail?.shopConfigs[0]?.partnerConfigId;
        setPartnerConfigId(_partnerConfigId);
      }
    }
  }, [detail]);

  useEffect(() => {
    setPartnerConfigId('');
  }, []);

  return (
    <Modal
      centered
      maskClosable={false}
      title={<h3 style={{ fontWeight: 'bold', marginBottom: '0' }}>Cửa Hàng</h3>}
      open={open}
      onCancel={handleClose}
      width={'80%'}
      footer={[
        <Button key="close" type="primary" onClick={handleClose}>
          Đóng
        </Button>,
      ]}
    >
      <Tabs
        defaultActiveKey="SHOP_INFO"
        activeKey={activeTab}
        type="card"
        size={'middle'}
        style={{ marginBottom: 32 }}
        items={[
          {
            key: 'SHOP_INFO',
            label: (
              <span>
                <InfoCircleOutlined /> Thông tin cửa hàng
              </span>
            ),
            children: <ShopInfo data={detail} onChange={onChange} />,
          },
          {
            disabled: !detail?.isVerified,
            key: 'SHOP_CONFIG',
            label: (
              <span>
                <SettingOutlined /> Kết nối đơn vị vận chuyển {totalConnected > 0 ? `(${totalConnected})` : ''}
              </span>
            ),
            children: (
              <ShopConfig
                callback={callback}
                shopId={detail?.id}
                partners={detail?.partners}
                ghnShopDetails={detail?.ghnShopDetails}
                shopConfigs={detail?.shopConfigs}
              />
            ),
          },
          {
            key: 'SHOP_DELIVERY_PRICE_PLANE',
            label: (
              <span>
                <SettingOutlined /> Bảng giá
              </span>
            ),
            disabled: !detail?.isVerified || !Boolean(partnerConfigId),
            children: <ShopDeliveryPricePlane ghnShopDetails={detail?.ghnShopDetails} partnerConfigId={partnerConfigId} shopId={detail?.id} />,
          },
        ]}
        onChange={handleTabChange}
      />
    </Modal>
  );
};

export default CustomerDetail;
