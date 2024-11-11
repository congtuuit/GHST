import type { IOrderPagedParameter, IPaginationResponse } from '@/interface/business';
import type { IOrderDetail, IOrderDto, IOrderViewDto, IUpdateOrderRequest, ShopOrderViewDto } from '@/interface/order/order.interface';
import type { RadioChangeEvent, TablePaginationConfig } from 'antd';
import type { FilterValue } from 'antd/es/table/interface';
import type { ColumnsType } from 'antd/lib/table';
import { Button, Card, Col, message, Radio, Row, Select, Tag, Typography } from 'antd';
import { useEffect, useRef, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import {
  apiCancelOrderGhn,
  apiConfirmOrderGhn,
  apiCountOrderByStatus,
  apiGetOrderDetail,
  apiGetOrders,
  apiGetShopDetail,
  apiGetShopOrders,
  apiUpdateGhnOrder,
} from '@/api/business.api';
import Datatable, { DatatableRef } from '@/components/core/datatable';
import { supplierKeys, suppliers } from '@/constants/data';
import { debounce, formatUtcToLocalTime } from '@/utils/common';
import OrderDetailDialog from './ghn/order-detail';
import GoBackButton from '@/components/core/GoBackButton';
import OrderStatus from './OrderStatus';
import { revertDateFormatMap } from '@/components/core/table-column/type';
import AdminOrderFilterWrapper from './AdminOrderFilterWrapper';
import { useParams } from 'react-router-dom';
import ghnOrderFilter, { FilterStatusOption } from '@/features/order/ghnOrderFilter';
import './ShopOrder.css';
import ChangeOrderWeight from './ghn/ChangeOrderWeight';
import NumberFormatter from '@/components/core/NumberFormatter';
import { addConfirmOrderToQueue } from '@/features/order/orderSlice';
import { handleCallApiConfirmOrder } from '@/features/order';

const { Option } = Select;

interface ShopOrdersProps {
  shopId: string | undefined;
}

const _ghnOrderFilter = new ghnOrderFilter();
const orderStatusSection = _ghnOrderFilter.filterStatus;

const ShopOrders = (props: ShopOrdersProps) => {
  const dispatch = useDispatch();
  const { shopId } = useParams(); // Destructure shopId from useParams
  const { orderFilter, confirmOrderQueue } = useSelector(state => state.order);

  const datatableRef = useRef<DatatableRef>(null);
  const [groupStatusFilterOptions, setGroupStatusFilterOptions] = useState<FilterStatusOption[]>(orderStatusSection);
  const [orderPagination, setOrderPagination] = useState<IPaginationResponse<IOrderViewDto> | null>(null);
  const [orderDetail, setOrderDetail] = useState<IOrderDetail | undefined>();
  const [tablePaginationConfig, setTablePaginationConfig] = useState<TablePaginationConfig>();
  const [tableFilters, setTableFilters] = useState<Record<string, FilterValue | null>>();
  const [orderStatusFilter, setOrderStatusFilter] = useState<number>(orderStatusSection[0].value);
  const [supplierSelected, setSupplierSelected] = useState<string>(supplierKeys.GHN);
  const [shopDetail, setShopDetail] = useState<any>(null);
  const [fetchingData, setFetchingData] = useState(false);
  const [openChangeOrderDialog, setOpenChangeOrderDialog] = useState(false);
  const [updateOrder, setUpdateOrder] = useState<IOrderViewDto | undefined>();
  const [reloadTable, setReloadTable] = useState(false);
  const [searchOrderCodes, setSearchOrderCodes] = useState<string>('');
  const [selectedOrders, setSelectedOrders] = useState<IOrderViewDto[]>([]);

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
      setReloadTable(!reloadTable);
    }
  };

  const handleConfirmOrder = async (orderId: string) => {
    if (Boolean(orderId)) {
      const response = await apiConfirmOrderGhn(orderId);
      if (response.success) {
        message.success('Đã xác nhận đơn hàng thành công!');
        setReloadTable(!reloadTable);
      } else {
        message.error('Xảy ra lỗi');
      }
    }
  };

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

  const handleChangeOrderWeight = (record: IOrderViewDto) => {
    setUpdateOrder(record);
    setOpenChangeOrderDialog(true);
  };

  const handleSubmitChangeOrderWeight = async (record: IOrderViewDto | undefined, newValues: any) => {
    const payload: IUpdateOrderRequest = {
      orderId: record?.id,
      length: newValues.length,
      width: newValues.width,
      height: newValues.height,
      convertRate: newValues.convertRate,
    };
    const response = await apiUpdateGhnOrder(payload);
    if (response.success) {
      setReloadTable(!reloadTable);
      setOpenChangeOrderDialog(false);
    }
  };

  const handleConfirmOrders = async () => {
    const orders = selectedOrders;
    const orderIds = orders.map(i => i.id);
    if (orderIds.length > 0) {
      dispatch(addConfirmOrderToQueue(orderIds));
    }
  };

  const handleClearSelection = () => {
    datatableRef.current?.clearSelectedRows();
    setSelectedOrders([]);
  };

  const handleSearchOrder = debounce(changedValues => {
    setSearchOrderCodes(changedValues);
  }, 300);

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
      width: '250px',
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
      width: '250px',
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
      title: 'KL Đơn hàng (gram)',
      dataIndex: '#donhang',
      key: '#donhang',
      render: (value: string, record: IOrderViewDto) => {
        const isCustomized = record.rootConvertedWeight !== record.convertedWeight;
        return (
          <div>
            <div>
              <Tag style={{ minWidth: '50px', textAlign: 'center' }} color={'gray'}>
                {<NumberFormatter value={record?.rootConvertedWeight} style="unit" unit="gram" />} [{record.rootDisplaySize}]
              </Tag>
            </div>
            {isCustomized && (
              <div style={{ marginTop: '10px' }}>
                <Tag style={{ minWidth: '50px', textAlign: 'center' }} color={'green'}>
                  {<NumberFormatter value={record?.convertedWeight} style="unit" unit="gram" />} [{record.displaySize}]
                </Tag>
              </div>
            )}
            {record.status === 'waiting_confirm' && (
              <Button onClick={() => handleChangeOrderWeight(record)} type="link">
                Thay đổi
              </Button>
            )}
          </div>
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
      render: (_: any, record: IOrderViewDto) => {
        if (record.status === 'waiting_confirm') {
          const isConfirming = confirmOrderQueue.indexOf(record.id) >= 0;
          return (
            <div key={record.id}>
              <Button
                loading={isConfirming}
                disabled={isConfirming}
                type="dashed"
                className="table-btn-action"
                size="middle"
                onClick={() => handleConfirmOrder(record.id)}
              >
                Xác nhận
              </Button>
              <Button disabled={isConfirming} danger className="table-btn-action" size="small" onClick={() => handleCancelOrder(record.id)}>
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

  useEffect(() => {
    fetchOrders({
      shopId: shopId,
      status: orderFilter?.status ?? '',
      deliveryPartner: supplierSelected ?? '',
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
  }, [shopId, supplierSelected, orderStatusFilter, orderFilter, tablePaginationConfig, reloadTable, searchOrderCodes]);

  useEffect(() => {
    fetchShopDetail();
  }, [shopId]);

  useEffect(() => {
    if (supplierSelected === supplierKeys.GHN) {
      fetchCountOrderByStatus(shopId as string);
    }
  }, [shopId, orderStatusFilter, supplierSelected]);

  useEffect(() => {
    if (confirmOrderQueue.length > 0) {
      handleCallApiConfirmOrder(() => {
        setReloadTable(!reloadTable);
        dispatch(addConfirmOrderToQueue([]));
      });
      handleClearSelection();
    }
  }, [confirmOrderQueue]);

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
            <Card>
              {/* Order list table */}
              <Datatable
                showSearch
                onSearch={handleSearchOrder}
                ref={datatableRef}
                mode="multiple"
                onSelectedRows={(selectedRows: any[]) => setSelectedOrders(selectedRows)}
                loading={fetchingData}
                columns={columns}
                dataSource={orderPagination}
                onChange={handleChangeTable}
                headerBox={
                  <div>
                    <AdminOrderFilterWrapper
                      style={{ marginTop: '10px', marginBottom: '10px' }}
                      styleContent={{ width: '200px' }}
                      selectedRows={selectedOrders.length}
                      handleConfirmOrders={handleConfirmOrders}
                      handleRefresh={() => setReloadTable(!reloadTable)}
                    />
                  </div>
                }
              />
            </Card>
          </Col>

          <OrderDetailDialog data={orderDetail} showSenderAddress={true} />
          <ChangeOrderWeight
            order={updateOrder}
            open={openChangeOrderDialog}
            onCancel={() => setOpenChangeOrderDialog(false)}
            onSubmit={handleSubmitChangeOrderWeight}
          />
        </Row>
      </Card>
    </div>
  );
};

export default ShopOrders;
