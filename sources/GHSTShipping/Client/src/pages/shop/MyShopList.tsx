// src/pages/MyShopList.tsx
import React, { useEffect, useState } from 'react';
import { Button, Card, Table, message } from 'antd';
import { ColumnsType } from 'antd/es/table';
import { BasicShopInfoDto, getShops } from '@/features/shop';
import { useSelector } from 'react-redux';
import { shopIdSelector } from '@/stores/user.store';
import ActionButton from '@/components/core/ActionButton';
import ShopModal from './ShopModal';

const MyShopList: React.FC = () => {
  const shopId = useSelector(shopIdSelector);
  const [shops, setShops] = useState<BasicShopInfoDto[]>([]);
  const [loading, setLoading] = useState<boolean>(false);
  const [isModalVisible, setIsModalVisible] = useState<boolean>(false);
  const [editData, setEditData] = useState<BasicShopInfoDto | null>(null);

  const columns: ColumnsType<BasicShopInfoDto> = [
    {
      title: 'STT',
      dataIndex: 'index',
      key: 'index',
      render: (_, __, index) => index + 1,
    },
    {
      title: 'Tên cửa hàng',
      dataIndex: 'name',
      key: 'name',
    },
    {
      title: 'Địa chỉ',
      dataIndex: 'address',
      key: 'address',
    },
    {
      title: 'Điện thoại',
      dataIndex: 'phoneNumber',
      key: 'phoneNumber',
    },
    {
      title: 'Thao tác',
      dataIndex: 'action',
      key: 'action',
      render: (_: any, record: BasicShopInfoDto) => {
        return (
          <div key={record.id}>
            <ActionButton onClick={() => handleEdit(record)} iconAction="edit" text="Chỉnh sửa" />
          </div>
        );
      },
    },
  ];

  const handleEdit = (record: BasicShopInfoDto) => {
    setEditData(record);
    setIsModalVisible(true);
  };

  const handleAddNew = () => {
    setEditData(null);
    setIsModalVisible(true);
  };

  const handleModalClose = () => {
    setIsModalVisible(false);
    setEditData(null);
  };

  const fetchShops = async () => {
    setLoading(true);
    try {
      const data = await getShops(shopId);
      setShops(data.data);
    } catch (error) {
      message.error('Lỗi khi tải danh sách cửa hàng');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchShops();
  }, []);

  return (
    <Card title="Danh sách cửa hàng">
      <Button type="primary" onClick={handleAddNew} style={{ marginBottom: '20px' }}>
        Thêm cửa hàng
      </Button>
      <Table columns={columns} dataSource={shops} rowKey={record => record.id as string} loading={loading} />
      <ShopModal visible={isModalVisible} onClose={handleModalClose} onSuccess={fetchShops} editData={editData} />
    </Card>
  );
};

export default MyShopList;
