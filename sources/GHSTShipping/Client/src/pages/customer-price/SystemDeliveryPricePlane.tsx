import React, { useState, useEffect } from 'react';
import { Table, Button, Modal, notification, Space } from 'antd';
import type { ColumnsType } from 'antd/es/table';
import DeliveryPricePlaneForm, { DeliveryPricePlaneFormDto } from './DeliveryPricePlaneForm';
import { request } from '@/api/base/request';

const SystemDeliveryPricePlane: React.FC = () => {
  const [data, setData] = useState<DeliveryPricePlaneFormDto[]>([]);
  const [loading, setLoading] = useState(false);
  const [isModalVisible, setIsModalVisible] = useState(false);

  const [formLoading, setFormLoading] = useState(false);

  // Fetch danh sách DeliveryPricePlanes
  const fetchDeliveryPricePlanes = async () => {
    setLoading(true);
    try {
      const response = await request('get', '/DeliveryPricePlane/List');
      setData(response.data || []);
    } catch (error) {
      notification.error({ message: 'Lỗi khi tải dữ liệu' });
    } finally {
      setLoading(false);
    }
  };

  // Gọi fetchDeliveryPricePlanes khi page load
  useEffect(() => {
    fetchDeliveryPricePlanes();
  }, []);

  // Xử lý submit form
  const handleCreate = async (formValues: DeliveryPricePlaneFormDto) => {
    setFormLoading(true);
    try {
      // Gọi API tạo mới
      const response = await request('post', '/DeliveryPricePlane/Upsert', formValues);
      if (response) {
        notification.success({ message: 'Tạo mới thành công!' });
        setIsModalVisible(false);
        fetchDeliveryPricePlanes(); // Reload lại bảng danh sách
      }
    } catch (error) {
      notification.error({ message: 'Lỗi khi tạo mới' });
    } finally {
      setFormLoading(false);
    }
  };

  // Định nghĩa các cột cho bảng
  const columns: ColumnsType<DeliveryPricePlaneFormDto> = [
    {
      title: 'Tên Bảng Giá',
      dataIndex: 'Name',
      key: 'Name',
    },
    {
      title: 'Shop ID',
      dataIndex: 'ShopId',
      key: 'ShopId',
    },
    {
      title: 'Trọng Lượng Tối Thiểu',
      dataIndex: 'MinWeight',
      key: 'MinWeight',
    },
    {
      title: 'Trọng Lượng Tối Đa',
      dataIndex: 'MaxWeight',
      key: 'MaxWeight',
    },
    {
      title: 'Giá Công Khai',
      dataIndex: 'PublicPrice',
      key: 'PublicPrice',
    },
    {
      title: 'Trạng Thái',
      dataIndex: 'IsActivated',
      key: 'IsActivated',
      render: value => (value ? 'Kích Hoạt' : 'Không Kích Hoạt'),
    },
  ];

  return (
    <div>
      <Space style={{ marginBottom: 16 }}>
        <Button type="primary" onClick={() => setIsModalVisible(true)}>
          Thêm mới
        </Button>
      </Space>

      {/* Bảng danh sách DeliveryPricePlanes */}
      <Table columns={columns} dataSource={data} loading={loading} rowKey={record => record.Id || ''} />

      {/* Modal hiển thị Form */}
      <Modal width={600} centered title="Tạo Bảng Giá Giao Hàng Mới" open={isModalVisible} onCancel={() => setIsModalVisible(false)} footer={null} destroyOnClose>
        <DeliveryPricePlaneForm onSubmit={(values: DeliveryPricePlaneFormDto) => handleCreate(values)} loading={formLoading} />
      </Modal>
    </div>
  );
};

export default SystemDeliveryPricePlane;
