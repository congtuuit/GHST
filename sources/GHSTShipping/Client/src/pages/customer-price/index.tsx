import type { ShopPricePlanDto } from '@/interface/business';
import type { FC } from 'react';

import './index.css';

import { ReloadOutlined } from '@ant-design/icons';
import { Button, Card, Col, message, Row, Select, Tabs } from 'antd';
import { useEffect, useState } from 'react';

import { apiCreateShopPricePlan, apiGetShops } from '@/api/business.api';

import PriceTable from './price-table';
import PriceConfigurationForm from './PriceConfigurationForm';
import SystemDeliveryPricePlane from './SystemDeliveryPricePlane';
import ShopDeliveryPricePlane from './ShopDeliveryPricePlane';

const { Option } = Select;

interface Shop {
  shopId: string;
  shopName: string;
}

const CustomerPricePage: FC = () => {
  const [shops, setShops] = useState<Shop[]>([]);
  const [selectedShop, setSelectedShop] = useState<string | undefined>(undefined);
  const [refreshTable, setRefreshTable] = useState<boolean>(false);

  // state to storage data to edit
  const [updateShopPricePlan, setUpdateShopPricePlan] = useState<ShopPricePlanDto>();

  const fetchShops = async () => {
    const { success, data } = await apiGetShops();

    if (success) {
      // Assuming data contains an array of shops
      setShops(
        data.data.map((shop: any) => ({
          shopId: shop.id,
          shopName: shop.shopUniqueCode + '_' + shop.shopName,
        })),
      );
    }
  };

  useEffect(() => {
    fetchShops();
  }, []);

  const handleChange = (value: string) => {
    setSelectedShop(value);
    console.log(`Selected shop ID: ${value}`); // Handle the selected shop ID as needed
  };

  const handleUpdatePriceConfig = async (values: ShopPricePlanDto, callback: any) => {
    if (!Boolean(selectedShop)) {
      message.info('Vui lòng chọn khách hàng muốn cấu hình!');
      callback && callback(false);

      return;
    }

    const { success } = await apiCreateShopPricePlan({ ...values, shopId: selectedShop });

    if (success) {
      message.success('Thành công!');
      setRefreshTable(!refreshTable);
    }

    callback && callback(success);
  };

  const handleEdit = (value: ShopPricePlanDto, callback: (success: boolean) => void) => {
    setUpdateShopPricePlan(value);
  };

  return (
    <Card>
      <Tabs defaultActiveKey="1">
        <Tabs.TabPane tab="Bảng giá chung" key="1">
          <SystemDeliveryPricePlane />
        </Tabs.TabPane>
      </Tabs>
    </Card>
  );
};

export default CustomerPricePage;
