import type { IOrderStatus } from './orderStatus';
import type { IOrderPagedParameter, IPaginationResponse } from '@/interface/business';
import type { IOrderDetail, IOrderDto, IOrderViewDto } from '@/interface/order/order.interface';
import type { TablePaginationConfig } from 'antd';
import type { FilterValue } from 'antd/es/table/interface';
import type { ColumnsType } from 'antd/lib/table';
import { SearchOutlined } from '@ant-design/icons';
import { Button, Card, Col, message, Radio, Row, Tag, Typography } from 'antd';
import { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import { apiCancelOrderGhn, apiCountOrderByStatus, apiGetOrderDetail, apiGetOrders, apiGetShopOrders } from '@/api/business.api';
import Datatable from '@/components/core/datatable';
import Price from '@/components/core/price';
import { debounce, formatUtcToLocalTime } from '@/utils/common';
import OrderDetailDialog from './components/ghn/order-detail';
import { orderStatuses } from './orderStatus';
import NumberFormatter from '@/components/core/NumberFormatter';
import OrderStatus from './components/OrderStatus';
import { revertDateFormatMap } from '@/components/core/table-column/type';
import ghnOrderFilter, { FilterStatusOption } from '@/features/order/ghnOrderFilter';
import { RadioChangeEvent } from 'antd/lib';
import AdminOrderFilterWrapper from './components/AdminOrderFilterWrapper';

const _ghnOrderFilter = new ghnOrderFilter();
const orderStatusSection = _ghnOrderFilter.filterStatus;

const ShopOrderList = () => {
  const { orderFilter, confirmOrderQueue } = useSelector(state => state.order);
  const { session } = useSelector(state => state.user);
  const shopId = session?.shopId;

  const [selectedSupplier, setSelectedSupplier] = useState<string>('');
  const [orderStatusFilter, setOrderStatusFilter] = useState<number>(orderStatusSection[0].value);
  const [orderPagination, setOrderPagination] = useState<IPaginationResponse<IOrderViewDto> | null>(null);
  const [orderDetail, setOrderDetail] = useState<IOrderDetail | undefined>();
  const [tablePaginationConfig, setTablePaginationConfig] = useState<TablePaginationConfig>();
  const [tableFilters, setTableFilters] = useState<Record<string, FilterValue | null>>();
  const [refresh, setRefresh] = useState<boolean>(false);
  const [searchOrderCodes, setSearchOrderCodes] = useState<string>('');
  const [groupStatusFilterOptions, setGroupStatusFilterOptions] = useState<FilterStatusOption[]>(orderStatusSection);
  const [selectedOrders, setSelectedOrders] = useState<IOrderViewDto[]>([]);
  const [reloadTable, setReloadTable] = useState(false);
  const [fetchingData, setFetchingData] = useState(false);

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
    setFetchingData(true);
    const response = await apiGetOrders(params);
    if (response.success) {
      setOrderPagination(response.data);
    }

    setFetchingData(false);
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
      render: (value: number, record: IOrderViewDto) => {
        if (record.status === 'waiting_confirm') {
          return (
            <div>
              <div>Tạm tính</div>
              <Price value={value} type="success" />
            </div>
          );
        }

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

  const handleSearchOrder = debounce(changedValues => {
    setSearchOrderCodes(changedValues);
  }, 300);

  const fetchCountOrderByStatus = async (shopId: string) => {
    let totalDraftOrder = 0;
    const __ = await apiGetShopOrders({
      pageSize: 1,
      pageNumber: 1,
      shopId: shopId,
    });

    if (__.success) {
      totalDraftOrder = __.data.data[0].totalDraftOrder;
    }

    const response = await apiCountOrderByStatus(shopId);
    if (response.success) {
      const ghnResponse = JSON.parse(response.data);
      if (ghnResponse.code === 200) {
        const ghnResponseData = ghnResponse.data;
        ghnResponseData['waiting_confirm'] = totalDraftOrder;
        const __groupStatusFilterOptions = _ghnOrderFilter.reCalculateFilterStatusTotals(ghnResponseData);
        setGroupStatusFilterOptions(__groupStatusFilterOptions);
      }
    }
  };

  // useEffect(() => {
  //   const params: IOrderPagedParameter = {
  //     pageNumber: (tablePaginationConfig?.current as number) ?? 1,
  //     pageSize: (tablePaginationConfig?.pageSize as number) ?? pageSize,
  //     deliveryPartner: selectedSupplier,
  //     orderCode: searchOrderCodes ?? '',
  //     status: orderStatusFilter ?? '',
  //   };

  //   fetchOrders(params);
  // }, [selectedSupplier, tablePaginationConfig, orderStatusFilter, refresh, searchOrderCodes]);

  useEffect(() => {
    if (shopId) {
      fetchCountOrderByStatus(shopId as string);
    }
  }, [shopId, orderStatusFilter]);

  useEffect(() => {
    fetchOrders({
      status: orderFilter?.status ?? '',
      pageNumber: tablePaginationConfig?.current ?? 1,
      pageSize: tablePaginationConfig?.pageSize ?? pageSize,
      orderCode: searchOrderCodes ?? '',

      groupStatus: orderStatusFilter,
      fromDate: orderFilter?.fromDate ?? '',
      toDate: orderFilter?.toDate ?? '',
      paymentTypeId: orderFilter?.paymentTypeId ?? '',
      isPrint: orderFilter?.isPrint ?? '',
      isCodFailedCollected: orderFilter?.isCodFailedCollected ?? '',
      isDocumentPod: orderFilter?.isDocumentPod ?? '',
    } as IOrderPagedParameter);
  }, [orderStatusFilter, orderFilter, tablePaginationConfig, reloadTable, searchOrderCodes]);

  return (
    <Card className="my-card-containter" title="Danh sách đơn hàng">
      <Row>
        <Col span={24} style={{ marginTop: '10px' }}>
          {groupStatusFilterOptions.length > 0 && (
            <Radio.Group
              className="order-status-filter-containter"
              style={{ display: 'flex', overflow: 'scroll', overflowY: 'hidden', overflowX: 'auto', width: '100%', whiteSpace: 'nowrap' }}
              onChange={(e: RadioChangeEvent) => setOrderStatusFilter(e.target.value)}
              value={orderStatusFilter}
              optionType="button"
            >
              {groupStatusFilterOptions?.map((i, key) => {
                return (
                  <Radio.Button key={key} value={i.value} style={{ width: 'fit-content', margin: '5px', borderRadius: '10px', fontSize: '16px' }}>
                    {i.name}
                    <Typography.Text strong type="danger">
                      {i.total > 0 && `(${i.total})`}
                    </Typography.Text>
                  </Radio.Button>
                );
              })}
            </Radio.Group>
          )}
        </Col>

        <Col span={24}>
          <Datatable
            showSearch
            loading={fetchingData}
            columns={columns}
            dataSource={orderPagination}
            onSearch={handleSearchOrder}
            onChange={handleChangeTable}
            headerBox={
              <div>
                <AdminOrderFilterWrapper
                  style={{ marginTop: '10px', marginBottom: '10px' }}
                  styleContent={{ width: '200px' }}
                  selectedRows={selectedOrders.length}
                  handleRefresh={() => setReloadTable(!reloadTable)}
                />
              </div>
            }
          />
        </Col>
      </Row>
      <OrderDetailDialog data={orderDetail} />
    </Card>
  );
};

export default ShopOrderList;
