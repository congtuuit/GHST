import React, { useState, useEffect, useRef } from 'react';
import { Table, Button, Modal, Space, message } from 'antd';
import type { ColumnsType } from 'antd/es/table';
import DeliveryPricePlaneForm from './DeliveryPricePlaneForm';
import { request } from '@/api/base/request';
import NumberFormatter from '@/components/core/NumberFormatter';
import { DeleteOutlined, EditOutlined } from '@ant-design/icons';
import { DeliveryPricePlaneFormDto } from '@/interface/shop';
const { confirm } = Modal;

const SystemDeliveryPricePlane: React.FC = () => {
  const formRef = useRef<any>();

  const [data, setData] = useState<DeliveryPricePlaneFormDto[]>([]);
  const [loading, setLoading] = useState(false);
  const [isModalVisible, setIsModalVisible] = useState(false);
  const [formLoading, setFormLoading] = useState(false);
  const [pricePlanceEditData, setPricePlanceEditData] = useState<DeliveryPricePlaneFormDto>();

  // Fetch danh sách DeliveryPricePlanes
  const fetchDeliveryPricePlanes = async () => {
    setLoading(true);
    try {
      const response = await request('get', '/DeliveryPricePlane/List');
      setData(response.data || []);
    } catch (error) {
      message.error('Lỗi khi tải dữ liệu');
    } finally {
      setLoading(false);
    }
  };

  const handleEditPricePlane = (record: DeliveryPricePlaneFormDto) => {
    setIsModalVisible(true);
    setPricePlanceEditData(record);
  };

  const handleDeletePricePlane = async (record: DeliveryPricePlaneFormDto, confirmed: boolean = false) => {
    if (confirmed) {
      const response = await request('delete', `/DeliveryPricePlane/delete/${record.id}`);
      if (response.success) {
        fetchDeliveryPricePlanes();
        message.success('Xóa thành công');
      }
    } else {
      confirm({
        title: 'Bạn có chắc chắn muốn xóa mục này?',
        okText: 'Đồng ý',
        okType: 'danger',
        cancelText: 'Hủy',
        centered: true,
        onOk() {
          handleDeletePricePlane(record, true);
        },
        onCancel() {},
      });
    }
  };

  // Gọi fetchDeliveryPricePlanes khi page load
  useEffect(() => {
    fetchDeliveryPricePlanes();
  }, []);

  useEffect(() => {
    if (isModalVisible) {
      formRef.current?.resetForm();
    }

    if (pricePlanceEditData && isModalVisible) {
      formRef.current?.setValues(pricePlanceEditData);
    }
  }, [pricePlanceEditData, isModalVisible]);

  // Xử lý submit form
  const handleCreate = async (formValues: DeliveryPricePlaneFormDto) => {
    setFormLoading(true);
    try {
      // Gọi API tạo mới
      const response = await request('post', '/DeliveryPricePlane/Upsert', formValues);
      if (response) {
        message.success('Tạo mới thành công!');
        setIsModalVisible(false);
        fetchDeliveryPricePlanes(); // Reload lại bảng danh sách
      }
    } catch (error) {
      message.error('Lỗi khi tạo mới');
    } finally {
      setFormLoading(false);
    }
  };

  // Định nghĩa các cột cho bảng
  const columns: ColumnsType<DeliveryPricePlaneFormDto> = [
    {
      title: 'Tên Bảng Giá',
      dataIndex: 'name',
      key: 'name',
    },
    {
      title: 'Trọng Lượng',
      dataIndex: 'minWeight',
      key: 'minWeight',
      render: (value: number, record: DeliveryPricePlaneFormDto) => {
        return (
          <span>
            <NumberFormatter value={value} />
            <span> - </span>
            <NumberFormatter value={record?.maxWeight} unit="gram" style="unit" />
          </span>
        );
      },
    },
    {
      title: 'Tỉ lệ quy đổi',
      dataIndex: 'convertWeightRate',
      key: 'convertWeightRate',
      align: 'right',
      render: (value: number) => {
        return <NumberFormatter value={value} />;
      },
    },
    {
      title: 'Giá Riêng',
      dataIndex: 'privatePrice',
      key: 'privatePrice',
      align: 'right',
      render: (value: number) => {
        return <NumberFormatter value={value} style="currency" currency="VND" locale="vi-VN" />;
      },
    },
    {
      title: 'Giá Công Khai',
      dataIndex: 'publicPrice',
      key: 'publicPrice',
      align: 'right',
      render: (value: number) => {
        return <NumberFormatter value={value} style="currency" currency="VND" locale="vi-VN" />;
      },
    },
    {
      title: 'Bước nhảy',
      dataIndex: 'step',
      key: 'step',
      width: '300px',
      render: (value: number, record: DeliveryPricePlaneFormDto) => {
        return (
          <div>
            <div>
              Mỗi {<NumberFormatter value={record.stepWeight} style="unit" unit="gram" />} vượt quá{' '}
              {<NumberFormatter value={record.maxWeight} style="unit" unit="gram" />}
            </div>
            <div>+ {<NumberFormatter value={record.stepPrice} style="currency" currency="VND" locale="vi-VN" />} </div>
          </div>
        );
      },
    },
    {
      title: 'Thao tác',
      key: 'action',
      width: 200,
      align: 'center' as const,
      render: (_: any, record: DeliveryPricePlaneFormDto) => {
        return (
          <div key={record.id}>
            <Button className="table-btn-action" size="small" onClick={() => handleEditPricePlane(record)}>
              <EditOutlined /> Sửa
            </Button>
            <Button danger className="table-btn-action" size="small" onClick={() => handleDeletePricePlane(record)}>
              <DeleteOutlined />
            </Button>
          </div>
        );
      },
    },
  ];

  return (
    <div>
      <Space style={{ marginBottom: 16 }}>
        <Button
          type="primary"
          onClick={() => {
            setIsModalVisible(true);
            setPricePlanceEditData({} as DeliveryPricePlaneFormDto);
          }}
        >
          Thêm mới
        </Button>
      </Space>

      {/* Bảng danh sách DeliveryPricePlanes */}
      <Table columns={columns} dataSource={data} loading={loading} rowKey={record => record.id || ''} />

      {/* Modal hiển thị Form */}
      <Modal width={600} centered title="Tạo Bảng Giá Giao Hàng Mới" open={isModalVisible} onCancel={() => setIsModalVisible(false)} footer={null}>
        <DeliveryPricePlaneForm ref={formRef} onSubmit={(values: DeliveryPricePlaneFormDto) => handleCreate(values)} loading={formLoading} />
      </Modal>
    </div>
  );
};

export default SystemDeliveryPricePlane;
