import type { IOrderStatus } from './orderStatus';
import type { IOrderPagedParameter, IPaginationResponse } from '@/interface/business';
import type { IOrderDetail, IOrderDto, IOrderViewDto } from '@/interface/order/order.interface';
import type { TablePaginationConfig } from 'antd';
import type { FilterValue } from 'antd/es/table/interface';
import type { ColumnsType } from 'antd/lib/table';
import { SearchOutlined } from '@ant-design/icons';
import { Button, Card, Col, message, Row, Tag, Typography } from 'antd';
import { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import { apiCancelOrderGhn, apiGetOrderDetail, apiGetOrders } from '@/api/business.api';
import Datatable from '@/components/core/datatable';
import Price from '@/components/core/price';
import { formatUtcToLocalTime } from '@/utils/common';
import OrderDetailDialog from './components/ghn/order-detail';
import { orderStatuses } from './orderStatus';
import NumberFormatter from '@/components/core/NumberFormatter';
import OrderStatus from './components/OrderStatus';
import { revertDateFormatMap } from '@/components/core/table-column/type';
import OrderFilter from './components/ghn/OrderFilter';

const ShopOrderList = () => {
  const { session } = useSelector(state => state.user);
  const [selectedSupplier, setSelectedSupplier] = useState<string>('');
  const [orderStatusSection, setOrderStatusSection] = useState<IOrderStatus[]>();
  const [orderStatusFilter, setOrderStatusFilter] = useState<string>();
  const [orderPagination, setOrderPagination] = useState<IPaginationResponse<IOrderDto> | null>(null);
  const [orderDetail, setOrderDetail] = useState<IOrderDetail | undefined>();
  const [tablePaginationConfig, setTablePaginationConfig] = useState<TablePaginationConfig>();
  const [tableFilters, setTableFilters] = useState<Record<string, FilterValue | null>>();
  const [refresh, setRefresh] = useState<boolean>(false);
  const pageSize = 7;

  const fetchOrders = async (params: IOrderPagedParameter | null) => {
    if (params == null) {
      params = {
        deliveryPartner: '',
        orderCode: '',
        status: '',
        pageNumber: 1,
        pageSize: pageSize,
      };
    }

    const response = await apiGetOrders(params);

    if (response.success) {
      setOrderPagination(response.data);
    }
  };

  const handleCancelOrder = async (id: string) => {
    const response = await apiCancelOrderGhn([id]);
    if (response.success) {
      setRefresh(!refresh);
      message.success('Hủy đơn thành công!');
    }
  };

  const columns: ColumnsType<IOrderViewDto> = [
    {
      title: 'STT',
      dataIndex: 'no',
      key: 'no',
      width: 50,
      align: 'center' as const,
    },
    {
      title: 'Mã đơn',
      dataIndex: 'clientOrderCode',
      key: 'clientOrderCode',
      width: 160,
      render: (value: string, record: IOrderViewDto) => {
        return (
          <div onClick={() => handleViewOrderDetail(record.id)} style={{ cursor: 'pointer' }}>
            <Button type="link">{value}</Button>
            <OrderStatus
              isPublished={record?.isPublished}
              status={record?.status}
              statusName={record?.statusName}
              statusColor={record?.statusColor}
            />
            <span>{record?.fromPhone}</span>
          </div>
        );
      },
    },
    {
      title: 'Người nhận',
      dataIndex: 'toName',
      key: 'toName',
      width: '200px',
      render: (value: string, record: IOrderViewDto) => {
        const dateFormatted = formatUtcToLocalTime(record?.created, revertDateFormatMap['day']);
        return (
          <div>
            <div>{value}</div>
            <div>{record?.toPhone}</div>
            <div style={{ fontStyle: 'italic', fontSize: '12px' }}>Ngày tạo: {dateFormatted}</div>
          </div>
        );
      },
    },
    {
      title: 'Địa chỉ nhận',
      dataIndex: 'toAddress',
      key: 'toAddress',
      width: '200px',
      render: (value: string, record: IOrderViewDto) => {
        return (
          <div>
            <div>{value}</div>
            <div>
              {record?.toWardName} - {record?.toDistrictName}
            </div>
            <div>{record?.toProvinceName}</div>
          </div>
        );
      },
    },
    {
      title: 'Thu hộ',
      dataIndex: 'codAmount',
      key: 'codAmount',
      align: 'right',
      render: (value: number) => {
        return (
          <div>
            <div style={{ fontSize: '12px' }}>COD</div>
            <Price style={{ fontWeight: 'bold' }} value={value} type="warning" />
          </div>
        );
      },
    },
    {
      title: 'Đơn hàng',
      dataIndex: 'insuranceValue',
      key: 'insuranceValue',
      render: (value: number, record: IOrderViewDto) => {
        return (
          <div>
            <div>
              Trọng lượng: <NumberFormatter value={record?.weight} style="unit" unit="gram" />
            </div>
            <div>
              Giá trị đơn hàng: <Price value={value} />
            </div>
          </div>
        );
      },
    },
    {
      title: 'Phí vận chuyển',
      dataIndex: 'deliveryFee',
      key: 'deliveryFee',
      align: 'right',
      render: (value: number) => {
        return <Price value={value} type="success" />;
      },
    },

    {
      title: 'Tùy chọn thanh toán',
      dataIndex: 'paymentTypeName',
      key: 'paymentTypeName',
      align: 'left',
      render: (value: string, record: IOrderViewDto) => {
        return (
          <div>
            <Tag style={{ minWidth: '50px', textAlign: 'center' }} color={record?.paymentTypeId === 1 ? '' : 'geekblue'}>
              {value}
            </Tag>
          </div>
        );
      },
    },
    {
      title: 'Trạng thái',
      align: 'center',
      render: (_: any, record: IOrderViewDto) => {
        const text = record.isPublished ? record.statusName : record.status === 'waiting_confirm' ? 'Chờ xác nhận' : record.statusName;
        return <Tag color={record.statusColor ?? 'gray'}>{text}</Tag>;
      },
    },
    {
      title: 'Thao tác',
      key: 'action',
      width: 150,
      align: 'center' as const,
      render: (_: any, record: IOrderViewDto) => {
        if (record.status === 'waiting_confirm' || record.status === 'draft' || record.status === 'ready_to_pick') {
          return (
            <div key={record.id}>
              <Button danger className="table-btn-action" size="small" onClick={() => handleCancelOrder(record.id)}>
                Hủy đơn
              </Button>
            </div>
          );
        }

        return <div key={record.id}></div>;
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

  const handleSearchOrder = async () => {};

  useEffect(() => {
    if (Boolean(selectedSupplier) && Boolean(orderStatuses[selectedSupplier])) {
      setOrderStatusSection(orderStatuses[selectedSupplier as string]);
    } else {
      setOrderStatusSection([]);
    }

    const params: IOrderPagedParameter = {
      pageNumber: (tablePaginationConfig?.current as number) ?? 1,
      pageSize: (tablePaginationConfig?.pageSize as number) ?? pageSize,
      deliveryPartner: selectedSupplier,
      orderCode: '',
      status: orderStatusFilter ?? '',
    };

    fetchOrders(params);
  }, [selectedSupplier, tablePaginationConfig, orderStatusFilter, refresh]);

  return (
    <Card className="my-card-containter" title="Danh sách đơn hàng">
      <Row>
        <Col span={24}>
          <Datatable
            onSearch={handleSearchOrder}
            showSearch
            columns={columns}
            dataSource={orderPagination}
            onChange={handleChangeTable}
          />
        </Col>
      </Row>
      <OrderDetailDialog data={orderDetail} />
    </Card>
  );
};

export default ShopOrderList;
