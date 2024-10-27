import { Card, Col } from 'antd';
import { useEffect, useState } from 'react';
import { supplierKeys } from '@/constants/data';
import FormOrderGhn from './components/ghn/form-order.ghn';
import { apiGetOrderMetaData } from '@/api/business.api';
import { IOrderMetadata } from '@/interface/shop';

const CreateOrderPage = () => {
  const [_, setCreateOrderMetadata] = useState<IOrderMetadata>();
  const [selectedSupplier, setSelectedSupplier] = useState<string>('');

  const handleChange = (value: string) => {
    setSelectedSupplier(value);
  };

  const fetchCreateOrderMetadata = async () => {
    const response = await apiGetOrderMetaData();
    if (response.success) {
      setCreateOrderMetadata(response.data);

      const config = response.data.deliveryConfigs[0];
      const partner = config.deliveryPartnerName;
      setSelectedSupplier(partner);

      if (partner === supplierKeys.GHN) {
        const shop = config.shops[0];
        const values = {
          name: shop.name,
          phone: shop.phone,
          address: shop.address,
          wardCode: shop.wardCode,
          districtId: shop.districtId,
        }
        
        localStorage.setItem('senderAddress', JSON.stringify(values));
      }
    }
  };

  useEffect(() => {
    fetchCreateOrderMetadata();
  }, []);

  return (
    <Card>
      <Col span={24}>{selectedSupplier === supplierKeys.GHN && <FormOrderGhn />}</Col>
    </Card>
  );
};

export default CreateOrderPage;
