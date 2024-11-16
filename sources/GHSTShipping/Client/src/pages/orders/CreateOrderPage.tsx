import { Card, Col } from 'antd';
import { useEffect, useState } from 'react';
import { supplierKeys } from '@/constants/data';
import GHN_CreateOrderForm from './components/ghn/GHN_CreateOrderForm';
import { apiGetOrderMetaData } from '@/api/business.api';
import { IOrderMetadata } from '@/interface/shop';

const CreateOrderPage = () => {
  const [isActiveGhnForm, setIsActiveGhnForm] = useState<boolean>(false);
  const [orderMeataData, setCreateOrderMetadata] = useState<IOrderMetadata>();

  const fetchCreateOrderMetadata = async () => {
    const response = await apiGetOrderMetaData();
    if (response.success) {
      setCreateOrderMetadata(response.data);
      const config = response.data.deliveryConfigs[0];
      const partner = config.deliveryPartnerName;
      if (partner === supplierKeys.GHN) {
        setIsActiveGhnForm(true);
        const shop = config.shops[0];
        localStorage.setItem('senderAddress', JSON.stringify(shop));
      }
    }
  };

  useEffect(() => {
    fetchCreateOrderMetadata();
  }, []);

  console.log('orderMeataData ', orderMeataData);

  return (
    <Card className="my-card-containter" title="Tạo đơn hàng">
      <Col span={24}>
        <GHN_CreateOrderForm
          isActivated={isActiveGhnForm}
          deliveryPricePlanes={orderMeataData?.deliveryPricePlanes}
          myShops={orderMeataData?.myShops}
        />
      </Col>
    </Card>
  );
};

export default CreateOrderPage;
