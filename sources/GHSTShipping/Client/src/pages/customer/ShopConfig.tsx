import type { IDeliveryParter, IGhnShopDetailDto, IShopConfig, IUpdateShopDeliveryConfigRequest } from '@/interface/shop';
import { Button, Card, Col, Row, Select, message } from 'antd';
import { ApiOutlined, DisconnectOutlined } from '@ant-design/icons';
import React, { useEffect, useMemo, useState } from 'react';
import { apiGetShopDetail, apiUpdateShopDeliveryConfig } from '@/api/business.api';
import { DeliveryParterName } from '@/constants/data';

interface ShopInfoProps {
  shopId?: string;
  partners?: IDeliveryParter[];
  ghnShopDetails?: { [id: string]: IGhnShopDetailDto[] };
  shopConfigs?: IShopConfig[];
}

interface PartnerConnectionProps {
  partner: IDeliveryParter;
  shopId?: string;
  ghnShopDetails?: { [id: string]: IGhnShopDetailDto[] };
  shopConfigs?: IShopConfig[];
  onChange?: () => void;
}

const PartnerConnection: React.FC<PartnerConnectionProps> = ({ partner, shopId, ghnShopDetails, shopConfigs, onChange }) => {
  const [isLoading, setIsLoading] = useState(false);
  const [ghnShopId, setGhnShopId] = useState('');
  const [clientPhone, setClientPhone] = useState('');
  const [isConnected, setIsConnected] = useState(false);

  const ghnShops = useMemo(() => ghnShopDetails?.[partner.partnerConfigId] || [], [partner.partnerConfigId, ghnShopDetails]);

  useEffect(() => {
    const connectedShop = shopConfigs?.find(config => config.partnerConfigId === partner.partnerConfigId);
    setIsConnected(Boolean(connectedShop));
    setGhnShopId(connectedShop?.partnerShopId || '');
    setClientPhone(connectedShop?.clientPhone || '');
  }, [partner.partnerConfigId, shopConfigs]);

  const updateConnection = async (isConnect: boolean) => {
    if (partner.partnerName === DeliveryParterName.GHN && !ghnShopId && isConnect) {
      message.error('Vui lòng chọn cửa hàng vận chuyển cần kết nối');
      return;
    }

    setIsLoading(true);
    const payload: IUpdateShopDeliveryConfigRequest = {
      shopId,
      deliveryConfigId: partner.partnerConfigId,
      partnerShopId: ghnShopId.toString(),
      isConnect,
      clientPhone,
    };
    const response = await apiUpdateShopDeliveryConfig(payload);
    if (response.success) {
      message.success(isConnect ? 'Kết nối thành công' : 'Hủy kết nối thành công');
      setIsConnected(isConnect);
      onChange && onChange();
    } else {
      message.error('Kết nối thất bại, vui lòng thử lại');
    }
    setIsLoading(false);
  };

  const handleChangeClientShop = (value: any) => {
    setGhnShopId(value);
    const shop = ghnShops.find(i => i.id === value);
    if (Boolean(shop)) {
      setClientPhone(shop?.phone as string);
    }
  };

  return (
    <Card style={{ width: '350px', boxShadow: '0 4px 8px rgba(0, 0, 0, 0.1)' }} className={isConnected ? 'connected' : ``}>
      <Row gutter={10}>
        <Col span={24}>
          <span>Đơn vị vận chuyển: {partner.partnerName}</span>
        </Col>
        <Col span={24}>
          <span>Tài khoản: {partner.partnerAccountName}</span>
        </Col>
        <Col span={24}>
          {ghnShops.length > 0 && (
            <Select
              disabled={isConnected}
              value={ghnShopId}
              showSearch
              placeholder="Chọn shop"
              style={{ width: '100%' }}
              onChange={handleChangeClientShop}
            >
              {ghnShops.map(shop => (
                <Select.Option key={shop.id} value={shop.id}>
                  {shop.displayName}
                </Select.Option>
              ))}
            </Select>
          )}
        </Col>
        <Col span={24} style={{ marginTop: 20 }}>
          {isConnected ? (
            <Button type="primary" loading={isLoading} onClick={() => updateConnection(false)}>
              <DisconnectOutlined /> Hủy kết nối
            </Button>
          ) : (
            <Button type="default" loading={isLoading} onClick={() => updateConnection(true)}>
              <ApiOutlined /> Kết nối
            </Button>
          )}
        </Col>
      </Row>
    </Card>
  );
};

const ShopConfig: React.FC<ShopInfoProps> = ({ shopId, partners, ghnShopDetails, shopConfigs }) => {
  const [currentShopConfigs, setCurrentShopConfigs] = useState(shopConfigs);

  const fetchShopDetail = async () => {
    const response = await apiGetShopDetail(shopId as string);
    if (response.success) {
      const detail = response.data;
      setCurrentShopConfigs(detail.shopConfigs);
    }
  };

  return (
    <Row gutter={[10, 10]} style={{ gap: '12px' }} className="shop-config">
      {partners?.map((partner, index) => (
        <PartnerConnection
          onChange={fetchShopDetail}
          key={index}
          partner={partner}
          shopId={shopId}
          ghnShopDetails={ghnShopDetails}
          shopConfigs={currentShopConfigs}
        />
      ))}
    </Row>
  );
};

export default ShopConfig;
