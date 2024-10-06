import { useEffect, useState, type FC } from 'react';
import {
  Badge,
  Button,
  Card,
  Checkbox,
  Col,
  Form,
  Input,
  Radio,
  RadioChangeEvent,
  Row,
  Select,
  Table,
  Tag,
} from 'antd';
import { useDispatch } from 'react-redux';

import { suppliers } from '@/constants/data';
import { IOrderStatus, orderStatuses } from './orderStatus';
import { IOrderDetail, IOrderDto } from '@/interface/order/order.interface';
import { apiGetOrderDetail, apiGetOrders } from '@/api/business.api';
import { IOrderPagedParameter, IPaginationResponse } from '@/interface/business';
import Datatable from '@/components/core/datatable';
import { ColumnsType } from 'antd/lib/table';
import Price from '@/components/core/price';
import { SearchOutlined } from '@ant-design/icons';
import OrderDetailDialog from './components/ghn/order-detail';

const { Option } = Select;

const OrdersPage = () => {
  const [selectedSupplier, setSelectedSupplier] = useState<string>('');
  const [orderStatusSection, setOrderStatusSection] = useState<IOrderStatus[]>();
  const [orderStatusFilter, setOrderStatusFilter] = useState<string>();
  const [orderPagination, setOrderPagination] = useState<IPaginationResponse<IOrderDto> | null>(null);
  const [orderDetail, setOrderDetail] = useState<IOrderDetail | undefined>();

  const handleChange = (value: string) => {
    setSelectedSupplier(value);
  };

  const handleSelectOrderStatusFilter = (e: RadioChangeEvent) => {
    setOrderStatusFilter(e.target.value);
  };

  const fetchOrders = async (params: IOrderPagedParameter | null) => {
    if (params == null) {
      params = {
        deliveryPartner: '',
        orderCode: '',
        pageNumber: 1,
        pageSize: 10,
      };
    }

    const response = await apiGetOrders(params);
    if (response.success) {
      setOrderPagination(response.data);
    }
  };

  const columns: ColumnsType<IOrderDto> = [
    {
      title: 'No',
      dataIndex: 'no',
      key: 'no',
      width: 50,
      align: 'center' as const,
    },
    {
      title: 'Mã đơn',
      dataIndex: 'clientOrderCode',
      key: 'clientOrderCode',
      width: 120,
      render: (value: string, record: IOrderDto) => {
        return (
          <Button type="link" onClick={() => handleViewOrderDetail(record.id)}>
            {value} <SearchOutlined />
          </Button>
        );
      },
    },
    {
      title: 'Địa chỉ gửi',
      dataIndex: 'fromAddress',
      key: 'fromAddress',
    },
    {
      title: 'Địa chỉ đến',
      dataIndex: 'toAddress',
      key: 'toAddress',
    },
    {
      title: 'Trọng lượng',
      dataIndex: 'weight',
      key: 'weight',
      width: 'auto',
      align: 'right',
    },
    {
      title: 'COD',
      dataIndex: 'codAmount',
      key: 'codAmount',
      align: 'right',
      render: (value: number) => {
        return <Price value={value} type="success" />;
      },
    },
    {
      title: 'InsuranceValue',
      dataIndex: 'insuranceValue',
      key: 'insuranceValue',
      align: 'right',
      render: (value: number) => {
        return <Price value={value} />;
      },
    },
    {
      title: 'Phí vận chuyển',
      dataIndex: 'deliveryFee',
      key: 'deliveryFee',
      align: 'right',
      render: (value: number) => {
        return <Price value={value} type="warning" />;
      },
    },
    {
      title: 'Trạng thái',
      align: 'center',
      render: (_: any, record: IOrderDto) => {
        const text = record.isPublished ? 'Công khai' : 'Nháp';
        return <Tag color={record['isPublished'] === true ? 'green' : 'gray'}>{text}</Tag>;
      },
    },
    {
      title: 'Thao tác',
      key: 'action',
      width: 150,
      align: 'center' as const,
      render: (_: any, record: IOrderDto) => {
        if (record.isPublished) {
          return (
            <div key={record.id}>
              <Button className="table-btn-action" size="small" onClick={() => {}}>
                Hủy đơn
              </Button>
            </div>
          );
        }
        return (
          <div key={record.id}>
            <Button className="table-btn-action" size="small" onClick={() => {}}>
              Công khai
            </Button>
            <Button className="table-btn-action" size="small" onClick={() => {}}>
              Xóa
            </Button>
          </div>
        );
      },
    },
  ];

  const handleViewOrderDetail = async (orderId: string) => {
    const response = await apiGetOrderDetail(orderId);
    if(response.success) {
      setOrderDetail(response.data)
    }
  };

  const handleChangeTable = (pagination: any, filters: any, sorter: any) => {};

  useEffect(() => {
    if (Boolean(selectedSupplier) && Boolean(orderStatuses[selectedSupplier])) {
      setOrderStatusSection(orderStatuses[selectedSupplier as string]);
    } else {
      setOrderStatusSection([]);
    }

    fetchOrders({
      deliveryPartner: selectedSupplier,
    } as IOrderPagedParameter);
  }, [selectedSupplier]);

  useEffect(() => {
    fetchOrders(null);
  }, []);

  return (
    <Card>
      <Row>
        <Col span={4}>
          <span>Đơn vị vận chuyển</span>
          <Select
            value={selectedSupplier}
            onChange={handleChange}
            style={{ width: '100%' }} // Adjust width as needed
            placeholder="Chọn đơn vị vận chuyển"
          >
            <Option key={'all'} value={''}>
              Tất cả
            </Option>
            {suppliers.map(i => (
              <Option key={i} value={i}>
                {i}
              </Option>
            ))}
          </Select>
        </Col>
        <Col span={24} style={{ marginTop: '10px' }}>
          {orderStatusSection && (
            <Radio.Group onChange={handleSelectOrderStatusFilter} value={orderStatusFilter}>
              {orderStatusSection?.map((i, key) => {
                return (
                  <Radio.Button key={key} value={i.code}>
                    {i.name}
                  </Radio.Button>
                );
              })}
            </Radio.Group>
          )}
        </Col>

        <Col span={24}>
          <Datatable columns={columns} dataSource={orderPagination} onChange={handleChangeTable} />
        </Col>
      </Row>
      <OrderDetailDialog data={orderDetail}/>
    </Card>
  );
};

export default OrdersPage;
