import { Card, Col } from 'antd';
import { useEffect, useState } from 'react';
import { supplierKeys } from '@/constants/data';
import GHN_CreateOrderForm from './components/ghn/GHN_CreateOrderForm';
import { apiGetOrderMetaData } from '@/api/business.api';
import { IOrderMetadata } from '@/interface/shop';
import { useParams } from 'react-router-dom';

const CreateOrderPage = () => {
  const [isActiveGhnForm, setIsActiveGhnForm] = useState<boolean>(false);
  const [orderMeataData, setCreateOrderMetadata] = useState<IOrderMetadata>();


  const { id } = useParams<{ id: string }>();
  const isEdit = Boolean(id) ? true : false;

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

  return (
    <Card className="my-card-containter" title={isEdit ? "Cập nhật đơn hàng": "Tạo đơn hàng"}>
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
