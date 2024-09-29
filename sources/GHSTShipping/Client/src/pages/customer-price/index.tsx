import { useState, lazy, type FC, useEffect } from 'react';

import { Card, Row, Typography, Select, Col, Form, Button, Table, message } from 'antd';

import { LocaleFormatter } from '@/locales';
import './index.css';
import { apiGetShops } from '@/api/user.api';
import { PlusOutlined } from '@ant-design/icons';
import PriceConfigurationForm from './PriceConfigurationForm';
import { apiCreateShopPricePlan, apiGetShopPricePlanes } from '@/api/business.api';
import { ShopPricePlanDto } from '@/interface/business';
import PriceTable from './price-table';

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
    <div>
      <Row style={{ flexFlow: 'row-reverse', marginBottom: '10px' }}>
        <Col span={12}>
          <span>Chọn khách hàng</span>
          <Select
            value={selectedShop}
            onChange={handleChange}
            style={{ width: '100%' }} // Adjust width as needed
            placeholder="Chọn khách hàng"
          >
            {shops.map(shop => (
              <Option key={shop.shopId} value={shop.shopId}>
                {shop.shopName}
              </Option>
            ))}
          </Select>
        </Col>
      </Row>
      <Row>
        <Card title="Cấu hình bảng giá" style={{ width: '100%' }}>
          <PriceConfigurationForm onSubmit={handleUpdatePriceConfig} data={updateShopPricePlan} />
          <Button onClick={() => setRefreshTable(!refreshTable)}>Làm mới</Button>
          <Row>
            <PriceTable refreshTable={refreshTable} selectedShop={selectedShop} onEdit={handleEdit} />
          </Row>
        </Card>
      </Row>
    </div>
  );
};

export default CustomerPricePage;
