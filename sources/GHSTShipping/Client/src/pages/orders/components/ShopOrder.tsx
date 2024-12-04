import type { IOrderPagedParameter, IPaginationResponse } from '@/interface/business';
import type { IOrderDetail, IOrderDto, IOrderViewDto, IUpdateOrderWeightRequest, ShopOrderViewDto } from '@/interface/order/order.interface';
import type { RadioChangeEvent, TablePaginationConfig } from 'antd';
import type { FilterValue } from 'antd/es/table/interface';
import type { ColumnsType } from 'antd/lib/table';
import { Button, Card, Col, message, Popover, Radio, Row, Select, Tag, Typography } from 'antd';
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
  apiUpdateGhnOrderWeight,
} from '@/api/business.api';
import Datatable, { DatatableRef } from '@/components/core/datatable';
import { supplierKeys, suppliers } from '@/constants/data';
import { debounce, formatUtcToLocalTime } from '@/utils/common';
import OrderDetailDialog from './ghn/order-detail';
import GoBackButton from '@/components/core/GoBackButton';
import OrderStatus from './OrderStatus';
import { revertDateFormatMap } from '@/components/core/table-column/type';
import AdminOrderFilterWrapper from './AdminOrderFilterWrapper';
import { useNavigate, useParams } from 'react-router-dom';
import ghnOrderFilter, { FilterStatusOption, OrderGroupStatus } from '@/features/order/ghnOrderFilter';
import './ShopOrder.css';
import ChangeOrderWeight from './ghn/ChangeOrderWeight';
import NumberFormatter from '@/components/core/NumberFormatter';
import { addConfirmOrderToQueue } from '@/features/order/orderSlice';
import { executeHandleEditOrder, handleCallApiConfirmOrder } from '@/features/order';
import Price from '@/components/core/price';
import CopyTextButton from '@/components/core/CopyTextButton';
import OrderDeliveryPlanDetail from './OrderDeliveryPlanDetail';
import { setStoreId } from '@/stores/user.store';
import ActionButton from '@/components/core/ActionButton';

const { Option } = Select;

interface ShopOrdersProps {
  shopId: string | undefined;
}

const _ghnOrderFilter = new ghnOrderFilter();
const orderStatusSection = _ghnOrderFilter.filterStatus;

const ShopOrders = (props: ShopOrdersProps) => {
  const navigate = useNavigate();
  const dispatch = useDispatch();
  const { shopId } = useParams(); // Destructure shopId from useParams
  const { confirmOrderQueue } = useSelector(state => state.order);
  const { orderFilter } = useSelector(state => state.orderTemp);

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
  const [formToken, setFormToken] = useState(0);

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

  /**
   * Admin đặt thông tin KL cho đơn hàng dùng để gủi sang đơn vị vận chuyên
   * Thông tin này sẽ không hiện ở Shop
   * @param record
   */
  const handleChangeOrderWeight = (record: IOrderViewDto) => {
    setUpdateOrder(record);
    setOpenChangeOrderDialog(true);
    setFormToken(formToken + 1);
  };

  const handleSubmitChangeOrderWeight = async (record: IOrderViewDto | undefined, newValues: any) => {
    const payload: IUpdateOrderWeightRequest = {
      orderId: record?.id,
      length: newValues.length,
      width: newValues.width,
      height: newValues.height,
      weight: newValues.weight,
    };
    const response = await apiUpdateGhnOrderWeight(payload);
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

  const handleCancelOrders = async () => {
    if (selectedOrders.length <= 0) {
      return;
    }

    const orders = selectedOrders;
    const orderIds = orders.map(i => i.id);
    const response = await apiCancelOrderGhn(orderIds);
    if (response.success) {
      message.success('Hủy đơn thành công!');
      setReloadTable(!reloadTable);
    }
  };

  const handleClearSelection = () => {
    datatableRef.current?.clearSelectedRows();
    setSelectedOrders([]);
  };

  const handleSearchOrder = debounce(changedValues => {
    setSearchOrderCodes(changedValues);
    setOrderStatusFilter(OrderGroupStatus.TatCa);
  }, 300);

  const handleEditOrder = async (record: IOrderViewDto) => {
    dispatch(setStoreId(record.shopId as string));
    executeHandleEditOrder(record, () => {
      navigate(`/order/update/${record.id}`);
    });
  };

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
          <div>
            <Button type="link">
              <span onClick={() => handleViewOrderDetail(record.id)}>{value} </span>
              <CopyTextButton text={value} />
            </Button>
            <div onClick={() => handleViewOrderDetail(record.id)}>
              <OrderStatus
                isPublished={record?.isPublished}
                status={record?.status}
                statusName={record?.statusName ?? 'N/A'}
                statusColor={record?.statusColor}
              />
              <span>{record?.fromPhone}</span>
            </div>
          </div>
        );
      },
    },
    {
      title: 'Người gửi',
      dataIndex: 'fromName',
      key: 'fromName',
      width: '120px',
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
      width: '200px',
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
      width: '100px',
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
      render: (value: number, record: IOrderViewDto) => {
        return (
          <div>
            <div style={{ fontSize: '12px' }}>COD</div>
            <Price style={{ fontWeight: 'bold' }} value={value} type="warning" />
            <div style={{ marginTop: '5px' }}>Giá trị đơn hàng</div>
            <Price value={record.insuranceValue} />
          </div>
        );
      },
    },
    {
      title: 'KL Đơn hàng (gram)',
      dataIndex: '#donhang',
      key: '#donhang',
      render: (value: string, record: IOrderViewDto) => {
        return (
          <div>
            <Tag style={{ minWidth: '50px', textAlign: 'center' }} color={record?.serviceTypeId === 2 ? 'cyan' : 'red'}>
              {record.serviceTypeName}
            </Tag>

            <div style={{ marginTop: '5px' }}>
              <span style={{ fontSize: '12px' }}>KL Tính Phí </span>
              <Tag style={{ minWidth: '50px', textAlign: 'center' }} color={'orange'}>
                {<NumberFormatter value={record?.calculateWeight} style="unit" unit="gram" />}
              </Tag>
            </div>

            <div style={{ marginTop: '5px' }}>
              <span style={{ fontSize: '12px' }}>KL Quy Đổi </span>
              <Tag style={{ minWidth: '50px', textAlign: 'center' }} color={''}>
                {<NumberFormatter value={record?.convertedWeight} style="unit" unit="gram" />}
              </Tag>
            </div>

            {/* {isCustomized && (
              <div style={{ marginTop: '10px' }}>
                <Tag style={{ minWidth: '50px', textAlign: 'center' }} color={'green'}>
                  {<NumberFormatter value={record?.convertedWeight} style="unit" unit="gram" />} [{record.displaySize}]
                </Tag>
              </div>
            )} */}

            <Popover
              content={
                <>
                  <div>
                    <div>KL Tính Phí: {<NumberFormatter value={record?.rootCalculateWeight} style="unit" unit="gram" />}</div>
                    <div>KL Quy Đổi: {<NumberFormatter value={record?.rootConvertedWeight} style="unit" unit="gram" />}</div>
                    <div>Cân nặng: {<NumberFormatter value={record?.rootWeight} style="unit" unit="gram" />}</div>
                    <div>
                      Kích thước: {<NumberFormatter value={record?.rootLength} style="decimal" />}x
                      {<NumberFormatter value={record?.rootWidth} style="decimal" />}x
                      {<NumberFormatter value={record?.rootHeight} style="unit" unit="centimeter" />}
                    </div>
                  </div>
                </>
              }
              title="Khối lượng đơn hàng khách nhập"
            >
              <Button size="small" type="link" style={{ fontSize: '12px', fontStyle: 'italic' }}>
                Thông tin khách nhập
              </Button>
            </Popover>

            <div>
              {record.status === 'waiting_confirm' && (
                <Button
                  title="Thay đổi KL đơn hàng, điều chỉnh thông tin gửi sang đơn vị vận chuyển"
                  onClick={() => handleChangeOrderWeight(record)}
                  type="link"
                >
                  Thay đổi
                </Button>
              )}
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
        return (
          <div>
            <div style={{ fontSize: '12px' }}>Tổng cước</div>
            <Price style={{ fontWeight: 'bold' }} value={record._TotalServiceFee} type="success" />
            <div>
              <Popover
                content={
                  <>
                    <div>
                      <span>Cước theo bảng giá: </span>
                      <Price value={value} />
                    </div>
                    <div>
                      <span>Phí bảo hiểm: </span>
                      <Price value={record.insuranceFee} />
                    </div>
                    <OrderDeliveryPlanDetail data={record.orderDeiveryPricePlanDetail} />
                  </>
                }
                title="Chi tiết cước"
              >
                <Button type="link">Xem chi tiết</Button>
              </Popover>
            </div>
          </div>
        );
      },
    },
    {
      title: 'Tùy chọn thanh toán',
      dataIndex: 'totalAmount',
      key: 'totalAmount',
      align: 'right',
      render: (value: number, record: IOrderViewDto) => {
        return (
          <>
            <div>
              <Tag style={{ minWidth: '50px', marginRight: '0' }} color={record?.paymentTypeId === 1 ? '' : 'geekblue'}>
                {record.paymentTypeName}
              </Tag>
            </div>

            <div>Tổng thu: </div>
            <Price value={value} type="success" />
            <div style={{ fontStyle: 'italic', fontSize: '12px' }}>(Bao gồm COD)</div>
          </>
        );
      },
    },
    {
      title: 'Thao tác',
      key: 'action',
      align: 'center' as const,
      width: 150,
      render: (_: any, record: IOrderViewDto) => {
        if (record.status === 'waiting_confirm') {
          const isConfirming = confirmOrderQueue.indexOf(record.id) >= 0;
          return (
            <div key={record.id}>
              <ActionButton
                style={{ marginBottom: '15px', width: '90px' }}
                iconAction="edit"
                text="Sửa đơn"
                onClick={() => handleEditOrder(record)}
              />

              <Button
                loading={isConfirming}
                disabled={isConfirming}
                type="dashed"
                className="table-btn-action"
                style={{ marginBottom: '15px', width: '90px' }}
                size="middle"
                onClick={() => handleConfirmOrder(record.id)}
              >
                Xác nhận
              </Button>

              <ActionButton
                style={{ marginBottom: '20px', width: '90px' }}
                iconAction="delete"
                danger
                text="Hủy đơn"
                onClick={() => handleCancelOrder(record.id)}
              />
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
  }, [shopId, orderStatusFilter, supplierSelected, reloadTable]);

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
                      isAdmin={true}
                      style={{ marginTop: '10px', marginBottom: '10px' }}
                      styleContent={{ width: '200px' }}
                      selectedRows={selectedOrders.length}
                      handleConfirmOrders={handleConfirmOrders}
                      handleRefresh={() => setReloadTable(!reloadTable)}
                      handleCancelOrders={handleCancelOrders}
                    />
                  </div>
                }
              />
            </Card>
          </Col>

          <OrderDetailDialog data={orderDetail} showSenderAddress={true} />
          <ChangeOrderWeight
            token={formToken}
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
