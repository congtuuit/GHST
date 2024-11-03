import type { IOrderPagedParameter, IPaginationResponse } from '@/interface/business';
import type { IOrderDetail, IOrderDto, IOrderViewDto, ShopOrderViewDto } from '@/interface/order/order.interface';
import type { RadioChangeEvent, TablePaginationConfig } from 'antd';
import type { FilterValue } from 'antd/es/table/interface';
import type { ColumnsType } from 'antd/lib/table';
import { Button, Card, Col, message, Radio, Row, Select, Tag } from 'antd';
import { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import { apiCancelOrderGhn, apiConfirmOrderGhn, apiGetOrderDetail, apiGetOrders, apiGetShopOrders } from '@/api/business.api';
import Datatable from '@/components/core/datatable';
import { suppliers } from '@/constants/data';
import { formatUtcToLocalTime } from '@/utils/common';
import { orderStatuses } from '../orderStatus';
import OrderDetailDialog from './ghn/order-detail';
import GoBackButton from '@/components/core/GoBackButton';
import OrderStatus from './OrderStatus';
import { revertDateFormatMap } from '@/components/core/table-column/type';

const { Option } = Select;

interface ShopOrdersProps {
  shopId: string | undefined;
}

const ShopOrders = ({ shopId }: ShopOrdersProps) => {
  const orderStatusSection = orderStatuses.GHN;

  const { shop } = useSelector(state => state.order);
  const [orderPagination, setOrderPagination] = useState<IPaginationResponse<IOrderDto> | null>(null);
  const [orderDetail, setOrderDetail] = useState<IOrderDetail | undefined>();
  const [tablePaginationConfig, setTablePaginationConfig] = useState<TablePaginationConfig>();
  const [tableFilters, setTableFilters] = useState<Record<string, FilterValue | null>>();
  const [orderStatusFilter, setOrderStatusFilter] = useState<string>(orderStatusSection[0].code);
  const [supplierSelected, setSupplierSelected] = useState<string>();

  const fetchOrders = async (params: IOrderPagedParameter | null) => {
    if (params == null) {
      params = {
        deliveryPartner: supplierSelected ?? '',
        orderCode: '',
        status: orderStatusFilter ?? '',
        pageNumber: tablePaginationConfig?.current ?? 1,
        pageSize: tablePaginationConfig?.pageSize ?? 8,
      };
    }

    console.log('params ', params);

    const response = await apiGetOrders(params);
    if (response.success) {
      setOrderPagination(response.data);
    }
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

  useEffect(() => {
    fetchOrders({
      shopId: shopId,
      deliveryPartner: supplierSelected ?? '',
      pageNumber: 1,
      pageSize: 6,
      status: orderStatusFilter,
    } as IOrderPagedParameter);
  }, [shopId, supplierSelected, orderStatusFilter]);

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
            <Button type="link" onClick={() => handleViewOrderDetail(record.id)}>
              {value}
            </Button>

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
      title: 'Địa chỉ',
      children: [
        {
          title: 'Gửi',
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
          title: 'Nhận',
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
      ],
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
            </div>
          );
        }

        if (record.status === 'cancel') {
          return <div key={record.id}></div>;
        }

        return (
          <div key={record.id}>
            <Button danger className="table-btn-action" size="small" onClick={() => handleCancelOrder(record.id)}>
              Hủy đơn
            </Button>
          </div>
        );
      },
    },
  ];

  return (
    <div>
      <GoBackButton />
      <Card className="my-card-containter" title={'Thông tin đơn hàng ' + shop?.name + `(${shop?.uniqueCode})`}>
        <Row>
          <Col span={6} style={{ display: 'flex' }}>
            <Select value={supplierSelected} onChange={setSupplierSelected} style={{ width: '100%' }} placeholder="Chọn đơn vị vận chuyển">
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
              <Radio.Group onChange={(e: RadioChangeEvent) => setOrderStatusFilter(e.target.value)} value={orderStatusFilter}>
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

          <Datatable columns={columns} dataSource={orderPagination} onChange={handleChangeTable} />

          <OrderDetailDialog data={orderDetail} />
        </Row>
      </Card>
    </div>
  );
};

export default ShopOrders;
