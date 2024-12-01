import { useState, useEffect, useRef, useMemo } from 'react';
import { Table, Button, Modal, Space, message, Select } from 'antd';
import type { ColumnsType } from 'antd/es/table';
import { request } from '@/api/base/request';
import NumberFormatter from '@/components/core/NumberFormatter';
import { DeleteOutlined } from '@ant-design/icons';
import { DeliveryPricePlaneFormDto, IGhnShopDetailDto } from '@/interface/shop';
const { confirm } = Modal;

interface ShopDeliveryPricePlaneProps {
  shopId: string | undefined;
  ghnShopDetails?: { [id: string]: IGhnShopDetailDto[] };
  partnerConfigId: string;
  disabled?: boolean;
}

const ShopDeliveryPricePlane = (props: ShopDeliveryPricePlaneProps) => {
  const { shopId, ghnShopDetails, partnerConfigId = '', disabled = false } = props;

  const formRef = useRef<any>();
  const [loading, setLoading] = useState(false);
  const [isModalVisible, setIsModalVisible] = useState(false);
  const [formLoading, setFormLoading] = useState(false);
  const [pricePlanceEditData, setPricePlanceEditData] = useState<DeliveryPricePlaneFormDto>();
  const [remainPricePlanes, setRemainPricePlanes] = useState<DeliveryPricePlaneFormDto[]>([]);
  const [shopPricePlanes, setShopPricePlanes] = useState<DeliveryPricePlaneFormDto[]>([]);

  const [selectedPricePlaneId, setSelectedPricePlaneId] = useState<string>('');
  const [selectedPartnerShopId, setSelectedPartnerShopId] = useState<string>('');

  const ghnShops = useMemo(() => ghnShopDetails?.[partnerConfigId] || [], [ghnShopDetails, partnerConfigId]);

  // Fetch danh sách DeliveryPricePlanes
  const fetchDeliveryPricePlanes = async () => {
    try {
      setLoading(true); // Đảm bảo set loading trước khi bắt đầu fetch dữ liệu

      // Thực hiện 2 request đồng thời
      const [response2, response] = await Promise.all([
        request('get', `/DeliveryPricePlane/List?ShopId=${shopId}`),
        request('get', `/DeliveryPricePlane/List`),
      ]);

      let _shopPricePlanes: DeliveryPricePlaneFormDto[] = [];
      if (response2.success) {
        _shopPricePlanes = response2.data.filter((i: DeliveryPricePlaneFormDto) => i.shopId === shopId);
        setShopPricePlanes(_shopPricePlanes);
      }

      if (response.success) {
        const shopPricePlaneIds = new Set(_shopPricePlanes.map((x: DeliveryPricePlaneFormDto) => x.parentId));
        const _remainPricePlanes = response.data.filter((i: DeliveryPricePlaneFormDto) => !shopPricePlaneIds.has(i.id));
        setRemainPricePlanes(_remainPricePlanes);
      }
    } catch (error) {
      console.error('Lỗi:', error);
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

  const handleSelectChange = (value: any) => {
    setSelectedPricePlaneId(value);
  };

  const handleSelectShopChange = (value: any) => {
    setSelectedPartnerShopId(value);
  };

  const handleAssignDeliveryPricePlaneToShop = async () => {
    const partnerShop: IGhnShopDetailDto | undefined = ghnShops.find(i => i.id === selectedPartnerShopId);
    const req = {
      shopId: shopId,
      deliveryPricePlaneId: selectedPricePlaneId,
      deliveryConfigId: partnerConfigId,
      partnerShopId: selectedPartnerShopId,
      clientPhone: partnerShop?.phone,
      address: partnerShop?.address,
      wardName: partnerShop?.wardName,
      districtName: partnerShop?.districtName,
      provineName: partnerShop?.provineName,
    };

    const response = await request('post', '/DeliveryPricePlane/Assign', req);
    console.log('response ', response);
    if (response.success) {
      fetchDeliveryPricePlanes();
      message.success('Thêm thành công');
    } else {
      message.error(response?.errors[0]?.description);
    }
  };

  // Gọi fetchDeliveryPricePlanes khi page load
  useEffect(() => {
    fetchDeliveryPricePlanes();
  }, [partnerConfigId, ghnShopDetails]);

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
      title: 'Mã shop',
      dataIndex: 'partnerShopId',
      key: 'partnerShopId',
      width: 120,
    },
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
      width: 200,
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
      align: 'center' as const,
      render: (_: any, record: DeliveryPricePlaneFormDto) => {
        return (
          <div key={record.id}>
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
        {ghnShops.length > 0 && (
          <Select
            key="shop"
            showSearch
            placeholder="Chọn shop"
            style={{ width: '300px' }}
            onChange={handleSelectShopChange}
            filterOption={(input: any, option: any) =>
              option?.value?.toString().toLowerCase().includes(input.toLowerCase()) ||
              option?.children?.toString().toLowerCase().includes(input.toLowerCase())
            }
          >
            {ghnShops.map(shop => (
              <Select.Option key={shop.id} value={shop.id}>
                {shop.displayName}
              </Select.Option>
            ))}
          </Select>
        )}

        <Select
          key={'price'}
          showSearch
          placeholder="Chọn bảng giá..."
          onChange={handleSelectChange}
          style={{ width: '300px' }}
          options={remainPricePlanes?.map(i => {
            return {
              value: i.id,
              label: i.name,
            };
          })}
        ></Select>
        <Button disabled={selectedPricePlaneId === '' || disabled} type="primary" onClick={handleAssignDeliveryPricePlaneToShop}>
          Thêm
        </Button>

        <Button type="dashed" onClick={fetchDeliveryPricePlanes}>
          Làm mới
        </Button>
      </Space>

      {/* Bảng danh sách DeliveryPricePlanes */}
      <Table columns={columns} dataSource={shopPricePlanes} loading={loading} rowKey={record => record.id || ''} />
    </div>
  );
};

export default ShopDeliveryPricePlane;
