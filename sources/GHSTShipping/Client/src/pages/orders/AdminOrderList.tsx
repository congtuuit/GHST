import type { IOrderStatus } from './orderStatus';
import type { IOrderPagedParameter, IPaginationResponse } from '@/interface/business';
import type { IOrderDetail, IOrderDto } from '@/interface/order/order.interface';
import type { RadioChangeEvent, TablePaginationConfig } from 'antd';
import type { FilterValue } from 'antd/es/table/interface';
import type { ColumnsType } from 'antd/lib/table';
import { SearchOutlined } from '@ant-design/icons';
import { Button, Card, Col, message, Radio, Row, Select, Tag } from 'antd';
import { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import { apiCancelOrderGhn, apiGetOrderDetail, apiGetOrders } from '@/api/business.api';
import Datatable from '@/components/core/datatable';
import Price from '@/components/core/price';
import { suppliers } from '@/constants/data';
import { commingSoon } from '@/utils/common';
import OrderDetailDialog from './components/ghn/order-detail';
import { orderStatuses } from './orderStatus';

const { Option } = Select;

const AdminOrderList = () => {
  const { session } = useSelector(state => state.user);
  const isAdmin = session.roles.includes('ADMIN');

  const [selectedSupplier, setSelectedSupplier] = useState<string>('');
  const [orderStatusSection, setOrderStatusSection] = useState<IOrderStatus[]>();
  const [orderStatusFilter, setOrderStatusFilter] = useState<string>();

  const [orderPagination, setOrderPagination] = useState<IPaginationResponse<IOrderDto> | null>(null);
  const [orderDetail, setOrderDetail] = useState<IOrderDetail | undefined>();

  const [tablePaginationConfig, setTablePaginationConfig] = useState<TablePaginationConfig>();
  const [tableFilters, setTableFilters] = useState<Record<string, FilterValue | null>>();

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
        status: '',
        pageNumber: 1,
        pageSize: 10,
      };
    }

    const response = await apiGetOrders(params);

    if (response.success) {
      setOrderPagination(response.data);
    }
  };

  const handleCancelOrder = async (orderCode: string | undefined) => {
    if (!Boolean(orderCode)) return;
    const response = await apiCancelOrderGhn([orderCode as string]);

    if (response.success) {
      message.success('Hủy đơn thành công!');
    }
  };

  const columns: ColumnsType<IOrderDto> = isAdmin
    ? [
        {
          title: 'No',
          dataIndex: 'no',
          key: 'no',
          width: 50,
          align: 'center' as const,
        },
        {
          title: 'Tên cửa hàng',
          dataIndex: 'shopName',
          key: 'shopName',
          width: 100,
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
          title: 'Giá trị đơn hàng',
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
          width: 180,
          align: 'center' as const,
          render: (_: any, record: IOrderDto) => {
            if (record.isPublished) {
              return (
                <div key={record.id}>
                  <Button className="table-btn-action" size="small" onClick={() => handleCancelOrder(record.orderCode)}>
                    Hủy đơn
                  </Button>
                </div>
              );
            }

            return (
              <div key={record.id}>
                <Button type="dashed" className="table-btn-action" size="middle" onClick={commingSoon}>
                  Công khai
                </Button>
                <Button danger className="table-btn-action" size="small" onClick={commingSoon}>
                  Xóa
                </Button>
              </div>
            );
          },
        },
      ]
    : [
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
                  <Button className="table-btn-action" size="small" onClick={commingSoon}>
                    Hủy đơn
                  </Button>
                </div>
              );
            }

            return (
              <div key={record.id}>
                <Button className="table-btn-action" size="small" onClick={commingSoon}>
                  Xóa
                </Button>
              </div>
            );
          },
        },
      ];

  const handleViewOrderDetail = async (orderId: string) => {
    const response = await apiGetOrderDetail(orderId);
    if (response.success) {
      setOrderDetail(response.data);
    }
  };

  const handleChangeTable = (config: TablePaginationConfig, filters: Record<string, FilterValue | null>) => {
    setTablePaginationConfig(config);
    setTableFilters(filters);
  };

  useEffect(() => {
    if (Boolean(selectedSupplier) && Boolean(orderStatuses[selectedSupplier])) {
      setOrderStatusSection(orderStatuses[selectedSupplier as string]);
    } else {
      setOrderStatusSection([]);
    }

    const params: IOrderPagedParameter = {
      pageNumber: tablePaginationConfig?.current as number ?? 1,
      pageSize: tablePaginationConfig?.pageSize as number ?? 10,
      deliveryPartner: selectedSupplier,
      orderCode: '',
      status: orderStatusFilter,
    };

    fetchOrders(params);
  }, [selectedSupplier, tablePaginationConfig, orderStatusFilter]);

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
      <OrderDetailDialog data={orderDetail} />
    </Card>
  );
};

export default AdminOrderList;
