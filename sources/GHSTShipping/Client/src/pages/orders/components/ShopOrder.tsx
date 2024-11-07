import type { IOrderPagedParameter, IPaginationResponse } from '@/interface/business';
import type { IOrderDetail, IOrderDto, IOrderViewDto, ShopOrderViewDto } from '@/interface/order/order.interface';
import type { RadioChangeEvent, TablePaginationConfig } from 'antd';
import type { FilterValue } from 'antd/es/table/interface';
import type { ColumnsType } from 'antd/lib/table';
import { Button, Card, Col, message, Radio, Row, Select, Tag, Typography } from 'antd';
import { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import { apiCancelOrderGhn, apiConfirmOrderGhn, apiCountOrderByStatus, apiGetOrderDetail, apiGetOrders, apiGetShopDetail } from '@/api/business.api';
import Datatable from '@/components/core/datatable';
import { supplierKeys, suppliers } from '@/constants/data';
import { formatUtcToLocalTime } from '@/utils/common';
import OrderDetailDialog from './ghn/order-detail';
import GoBackButton from '@/components/core/GoBackButton';
import OrderStatus from './OrderStatus';
import { revertDateFormatMap } from '@/components/core/table-column/type';
import AdminOrderFilterWrapper from './AdminOrderFilterWrapper';
import { useParams } from 'react-router-dom';
import ghnOrderFilter, { FilterStatusOption } from '@/features/order/ghnOrderFilter';
import './ShopOrder.css';

const { Option } = Select;

interface ShopOrdersProps {
  shopId: string | undefined;
}

const _ghnOrderFilter = new ghnOrderFilter();
const orderStatusSection = _ghnOrderFilter.filterStatus;

const ShopOrders = (props: ShopOrdersProps) => {
  const { shopId } = useParams(); // Destructure shopId from useParams
  const { orderFilter } = useSelector(state => state.order);

  const [groupStatusFilterOptions, setGroupStatusFilterOptions] = useState<FilterStatusOption[]>(orderStatusSection);
  const [orderPagination, setOrderPagination] = useState<IPaginationResponse<IOrderDto> | null>(null);
  const [orderDetail, setOrderDetail] = useState<IOrderDetail | undefined>();
  const [tablePaginationConfig, setTablePaginationConfig] = useState<TablePaginationConfig>();
  const [tableFilters, setTableFilters] = useState<Record<string, FilterValue | null>>();
  const [orderStatusFilter, setOrderStatusFilter] = useState<number>(orderStatusSection[0].value);
  const [supplierSelected, setSupplierSelected] = useState<string>(supplierKeys.GHN);
  const [shopDetail, setShopDetail] = useState<any>(null);
  const [fetchingData, setFetchingData] = useState(false);
  const pageSize = 5;

  const fetchShopDetail = async () => {
    const response = await apiGetShopDetail(shopId as string);
    if (response.success) {
      setShopDetail(response.data);
    }
  };

  const fetchOrders = async (params: IOrderPagedParameter | null) => {
    setFetchingData(true);

    if (params == null) {
      params = {
        deliveryPartner: supplierSelected ?? '',
        orderCode: '',
        groupStatus: orderStatusFilter ?? '',
        pageNumber: tablePaginationConfig?.current ?? 1,
        pageSize: tablePaginationConfig?.pageSize ?? pageSize,
      };
    }

    const response = await apiGetOrders(params);
    if (response.success) {
      setOrderPagination(response.data);
    }
    setFetchingData(false);
  };

  const handleChangeTable = (config: TablePaginationConfig, filters: Record<string, FilterValue | null>) => {
    setTablePaginationConfig(config);
    setTableFilters(filters);
  };

  const handleViewOrderDetail = async (orderId: string) => {
    const response = await apiGetOrderDetail(orderId);
    if (response.success) {
      setOrderDetail(response.data);
    }
  };

  const handleCancelOrder = async (orderCode: string | undefined) => {
    if (!Boolean(orderCode)) return;
    const response = await apiCancelOrderGhn([orderCode as string]);
    if (response.success) {
      message.success('Hủy đơn thành công!');
    }
  };

  const handleConfirmOrder = async (orderId: string) => {
    if (Boolean(orderId)) {
      const response = await apiConfirmOrderGhn(orderId);
      if (response.success) {
        message.success('Đã xác nhận đơn hàng thành công!');
        await fetchOrders(null);
      } else {
        message.error('Xảy ra lỗi');
      }
    }
  };

  const fetchCountOrderByStatus = async (shopId: string) => {
    const response = await apiCountOrderByStatus(shopId);
    if (response.success) {
      const ghnResponse = JSON.parse(response.data);

      if (ghnResponse.code === 200) {
        const ghnResponseData = ghnResponse.data;
        const __groupStatusFilterOptions = _ghnOrderFilter.reCalculateFilterStatusTotals(ghnResponseData);
        console.log('data ', ghnResponseData);
        console.log('__groupStatusFilterOptions ', __groupStatusFilterOptions);
        setGroupStatusFilterOptions(__groupStatusFilterOptions);
      }
    }
  };

  useEffect(() => {
    fetchOrders({
      shopId: shopId,
      status: orderFilter?.status ?? '',
      deliveryPartner: supplierSelected ?? '',
      pageNumber: tablePaginationConfig?.current ?? 1,
      pageSize: tablePaginationConfig?.pageSize ?? pageSize,

      groupStatus: orderStatusFilter,
      fromDate: orderFilter?.fromDate ?? null,
      toDate: orderFilter?.toDate ?? null,
      paymentTypeId: orderFilter?.paymentTypeId ?? '',
      isPrint: orderFilter?.isPrint ?? '',
      isCodFailedCollected: orderFilter?.isCodFailedCollected ?? '',
      isDocumentPod: orderFilter?.isDocumentPod ?? '',
    } as IOrderPagedParameter);
  }, [shopId, supplierSelected, orderStatusFilter, orderFilter, tablePaginationConfig]);

  useEffect(() => {
    fetchShopDetail();
  }, [shopId]);

  useEffect(() => {
    if (supplierSelected === supplierKeys.GHN) {
      fetchCountOrderByStatus(shopId as string);
    }
  }, [shopId, orderStatusFilter, supplierSelected]);

  const columns: ColumnsType<IOrderViewDto> = [
    {
      title: 'No',
      dataIndex: 'no',
      key: 'no',
      width: '50px',
      align: 'center' as const,
    },
    {
      title: 'Mã đơn',
      dataIndex: 'clientOrderCode',
      key: 'clientOrderCode',
      width: '50px',
      render: (value: string, record: IOrderViewDto) => {
        return (
          <div onClick={() => handleViewOrderDetail(record.id)}>
            <Button type="link">{value}</Button>
            <OrderStatus
              isPublished={record?.isPublished}
              status={record?.status}
              statusName={record?.statusName ?? 'N/A'}
              statusColor={record?.statusColor}
            />
            <span>{record?.fromPhone}</span>
          </div>
        );
      },
    },
    {
      title: 'Người gửi',
      dataIndex: 'fromName',
      key: 'fromName',
      width: '200px',
      render: (value: string, record: IOrderViewDto) => {
        const dateFormatted = formatUtcToLocalTime(record?.created, revertDateFormatMap['day']);
        return (
          <div>
            <div>{value}</div>
            <div>{record?.fromPhone}</div>
            <div style={{ fontStyle: 'italic', fontSize: '12px' }}>Ngày tạo: {dateFormatted}</div>
          </div>
        );
      },
    },
    {
      title: 'Địa chỉ gửi',
      dataIndex: 'fromAddress',
      key: 'fromAddress',
      render: (value: string, record: IOrderViewDto) => {
        return (
          <div>
            <div>{value}</div>
            <div>
              {record?.fromWardName} - {record?.fromDistrictName}
            </div>
            <div>{record?.fromProvinceName}</div>
          </div>
        );
      },
    },
    {
      title: 'Người nhận',
      dataIndex: 'toName',
      key: 'toName',
      width: '150px',
      render: (value: string, record: IOrderViewDto) => {
        return (
          <div>
            <div>{value}</div>
            <div>{record?.toPhone}</div>
          </div>
        );
      },
    },
    {
      title: 'Địa chỉ nhận',
      dataIndex: 'toAddress',
      key: 'toAddress',
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
      title: 'Loại dịch vụ',
      dataIndex: 'serviceTypeName',
      key: 'serviceTypeName',
      width: '100px',
      render: (value: string, record: IOrderViewDto) => {
        return (
          <Tag style={{ minWidth: '50px', textAlign: 'center' }} color={record?.serviceTypeId === 2 ? 'cyan' : 'red'}>
            {value}
          </Tag>
        );
      },
    },
    {
      title: 'Người thanh toán',
      dataIndex: 'paymentTypeName',
      key: 'paymentTypeName',
      width: '100px',
      render: (value: string, record: IOrderViewDto) => {
        return (
          <Tag style={{ minWidth: '50px', textAlign: 'center' }} color={record?.paymentTypeId === 1 ? '' : 'geekblue'}>
            {value}
          </Tag>
        );
      },
    },
    {
      title: 'Thao tác',
      key: 'action',
      align: 'center' as const,
      width: '190px',
      render: (_: any, record: IOrderViewDto) => {
        if (record.status === 'waiting_confirm') {
          return (
            <div key={record.id}>
              <Button type="dashed" className="table-btn-action" size="middle" onClick={() => handleConfirmOrder(record.id)}>
                Xác nhận
              </Button>
              <Button danger className="table-btn-action" size="small" onClick={() => handleCancelOrder(record.id)}>
                Hủy đơn
              </Button>
            </div>
          );
        }

        if (record.status === 'cancel') {
          return <div key={record.id}></div>;
        }

        if (record.status === 'ready_to_pick' || record.status === 'picking' || record.status === 'money_collect_picking') {
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

  return (
    <div>
      <GoBackButton />
      <Card className="my-card-containter" title={'Thông tin đơn hàng ' + shopDetail?.shopName + `(${shopDetail?.shopUniqueCode})`}>
        <Row>
          <Col span={6} style={{ display: 'flex' }}>
            <Select value={supplierSelected} onChange={setSupplierSelected} style={{ width: '100%' }} placeholder="Chọn đơn vị vận chuyển">
              {suppliers.map(i => (
                <Option key={i} value={i}>
                  {i}
                </Option>
              ))}
            </Select>
          </Col>
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
                      {i.name}{' '}
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
            {/* Order list table */}
            <Datatable
              loading={fetchingData}
              columns={columns}
              dataSource={orderPagination}
              onChange={handleChangeTable}
              headerBox={<AdminOrderFilterWrapper style={{ marginTop: '10px', marginBottom: '10px' }} styleContent={{ width: '200px' }} />}
            />
          </Col>

          <OrderDetailDialog data={orderDetail} showSenderAddress={true} />
        </Row>
      </Card>
    </div>
  );
};

export default ShopOrders;
