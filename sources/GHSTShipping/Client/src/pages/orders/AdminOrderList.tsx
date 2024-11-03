import type { IPaginationResponse } from '@/interface/business';
import type { IOrderDto, ShopOrderViewDto } from '@/interface/order/order.interface';
import type { TablePaginationConfig } from 'antd';
import type { FilterValue } from 'antd/es/table/interface';
import type { ColumnsType } from 'antd/lib/table';
import { SearchOutlined } from '@ant-design/icons';
import { Button, Card, message, Tag } from 'antd';
import { useEffect, useState } from 'react';
import { apiCancelOrderGhn, apiGetShopOrders } from '@/api/business.api';
import Datatable from '@/components/core/datatable';
import { commingSoon } from '@/utils/common';
import { IPaginationRequestParameter } from '@/interface';
import { useDispatch, useSelector } from 'react-redux';
import { setShopId } from '@/features/order/orderSlice';
import MyModal from '@/components/core/modal';
import ShopOrders from './components/ShopOrder';

const AdminOrderList = () => {
  const dispatch = useDispatch();
  const [shopOrderPagination, setShopOrderPagination] = useState<IPaginationResponse<ShopOrderViewDto> | null>(null);
  const [tablePaginationConfig, setTablePaginationConfig] = useState<TablePaginationConfig>();
  const [openOrders, setOpenOrders] = useState<boolean>(false);
  const [shopSelected, setShopSelected] = useState<ShopOrderViewDto>();

  const fetchShopOrders = async (params: IPaginationRequestParameter | null) => {
    if (params == null) {
      params = {
        pageNumber: 1,
        pageSize: 10,
      };
    }

    const response = await apiGetShopOrders(params);

    if (response.success) {
      setShopOrderPagination(response.data);
    }
  };

  const handleCancelOrder = async (orderCode: string | undefined) => {
    if (!Boolean(orderCode)) return;
    const response = await apiCancelOrderGhn([orderCode as string]);
    if (response.success) {
      message.success('Hủy đơn thành công!');
    }
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
          <Button
            type="link"
            onClick={() => {
              setShopSelected(record);
              setOpenOrders(true);
              dispatch(setShopId(record.id));
            }}
          >
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
          <Button
            type="link"
            onClick={() => {
              setShopSelected(record);
              setOpenOrders(true);
              dispatch(setShopId(record.id));
            }}
          >
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
            <Button
              type="link"
              className="table-btn-action"
              size="middle"
              onClick={() => {
                setShopSelected(record);
                setOpenOrders(true);
                dispatch(setShopId(record.id));
              }}
            >
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
      pageSize: (tablePaginationConfig?.pageSize as number) ?? 10,
    };

    fetchShopOrders(params);
  }, [tablePaginationConfig]);

  return (
    <Card>
      <Datatable columns={columns} dataSource={shopOrderPagination} onChange={handleChangeTable} />
      <MyModal
        title={`Đơn hàng - ${shopSelected?.name} (${shopSelected?.uniqueCode})`}
        open={openOrders}
        onClose={() => setOpenOrders(false)}
        maskClosable={false}
        width={"80%"}
        height={"100%"}
        centered
        footer={[
          <Button key="ok" type="primary" onClick={() => setOpenOrders(false)}>
            Đóng
          </Button>,
        ]}
      >
        <ShopOrders />
      </MyModal>
    </Card>
  );
};

export default AdminOrderList;
