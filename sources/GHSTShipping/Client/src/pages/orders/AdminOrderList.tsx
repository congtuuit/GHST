import type { IPaginationResponse } from '@/interface/business';
import type { ShopOrderViewDto } from '@/interface/order/order.interface';
import type { TablePaginationConfig } from 'antd';
import type { FilterValue } from 'antd/es/table/interface';
import type { ColumnsType } from 'antd/lib/table';
import { Button, Card, message, Popover, Tag } from 'antd';
import { useEffect, useState } from 'react';
import { apiCancelOrderGhn, apiGetShopOrders } from '@/api/business.api';
import Datatable from '@/components/core/datatable';
import { IPaginationRequestParameter } from '@/interface';
import { useDispatch } from 'react-redux';
import { setShopInfo } from '@/features/order/orderSlice';
import { useNavigate } from 'react-router-dom';
import OrderFilter from './components/ghn/OrderFilter';
import AdminOrderFilterWrapper from './components/AdminOrderFilterWrapper';

const AdminOrderList = () => {
  const dispatch = useDispatch();
  const navigate = useNavigate();
  const [shopOrderPagination, setShopOrderPagination] = useState<IPaginationResponse<ShopOrderViewDto> | null>(null);
  const [tablePaginationConfig, setTablePaginationConfig] = useState<TablePaginationConfig>();
  const [shopSelected, setShopSelected] = useState<ShopOrderViewDto>();

  const fetchShopOrders = async (params: IPaginationRequestParameter | null) => {
    if (params == null) {
      params = {
        pageNumber: 1,
        pageSize: 8,
      };
    }

    const response = await apiGetShopOrders(params);
    if (response.success) {
      setShopOrderPagination(response.data);
    }
  };

  const handleViewShopOrders = (record: ShopOrderViewDto) => {
    setShopSelected(record);
    dispatch(setShopInfo(record));
    navigate(record.id);
  };

  const columns: ColumnsType<ShopOrderViewDto> = [
    {
      title: 'No',
      dataIndex: 'no',
      key: 'no',
      width: 50,
      align: 'center' as const,
    },
    {
      title: 'Mã cửa hàng',
      dataIndex: 'uniqueCode',
      key: 'uniqueCode',
      width: 120,
      render: (value: string, record: ShopOrderViewDto) => {
        return (
          <Button type="link" onClick={() => handleViewShopOrders(record)}>
            {value}
          </Button>
        );
      },
    },
    {
      title: 'Tên cửa hàng',
      dataIndex: 'name',
      key: 'name',
      align: 'center' as const,
      render: (value: string, record: ShopOrderViewDto) => {
        return (
          <Button type="link" onClick={() => handleViewShopOrders(record)}>
            {value}
          </Button>
        );
      },
    },
    {
      title: 'Địa chỉ',
      dataIndex: 'address',
      key: 'address',
    },
    {
      title: 'Đơn đã xác nhận',
      dataIndex: 'totalPublishedOrder',
      key: 'totalPublishedOrder',
      width: 'auto',
      align: 'right',
      render: (value: number) => {
        return (
          <Tag style={{ minWidth: '50px', textAlign: 'center' }} color={'green'}>
            {value}
          </Tag>
        );
      },
    },
    {
      title: 'Đơn chờ xác nhận',
      dataIndex: 'totalDraftOrder',
      key: 'totalDraftOrder',
      align: 'right',
      render: (value: number) => {
        return (
          <Tag style={{ minWidth: '50px', textAlign: 'center' }} color={'gray'}>
            {value}
          </Tag>
        );
      },
    },

    {
      title: 'Thao tác',
      key: 'action',
      align: 'center' as const,
      render: (_: any, record: ShopOrderViewDto) => {
        return (
          <div key={record.id}>
            <Button type="link" className="table-btn-action" size="middle" onClick={() => handleViewShopOrders(record)}>
              Xem đơn hàng
            </Button>
          </div>
        );
      },
    },
  ];

  const handleChangeTable = (config: TablePaginationConfig, filters: Record<string, FilterValue | null>) => {
    setTablePaginationConfig(config);
  };

  useEffect(() => {
    const params: IPaginationRequestParameter = {
      pageNumber: (tablePaginationConfig?.current as number) ?? 1,
      pageSize: (tablePaginationConfig?.pageSize as number) ?? 8,
    };

    fetchShopOrders(params);
  }, [tablePaginationConfig]);

  return (
    <Card>
      <Datatable columns={columns} dataSource={shopOrderPagination} onChange={handleChangeTable} />
    </Card>
  );
};

export default AdminOrderList;
