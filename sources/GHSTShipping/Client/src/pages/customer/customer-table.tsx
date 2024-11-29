import type { PaginationResponse } from '@/interface/business';
import type { IShopViewDetailDto } from '@/interface/shop';
import type { TablePaginationConfig } from 'antd';
import type { FilterValue } from 'antd/es/table/interface';

import { CheckCircleOutlined, SearchOutlined } from '@ant-design/icons';
import { Button, InputNumber, Row, Table, Tag } from 'antd';
import dayjs from 'dayjs';
import { useEffect, useState } from 'react';

import { apiActiveShops, apiChangeOperationConfig, apiGetShopDetail, apiGetShops } from '@/api/business.api';
import { dateFormatMap, revertDateFormatMap } from '@/components/core/table-column/type';

import CustomerDetail from './customer-detail';
import { render } from 'react-dom';
import CommaDecimalDisplay from '@/components/core/CommaDecimalDisplay';
import { IChangeOperationConfig } from '@/api/type';

interface ShopDatatableDto {
  key: string;
  id: string;
  code: string;
  registerDate: string;
  shopName: string;
  fullName: string;
  email: string;
  avgMonthlyCapacity: number;
  status: string;
  isVerified: boolean;
  totalDeliveryConnected: number;
}

const CustomerTable = () => {
  const [paginationResponse, setPaginationResponse] = useState<PaginationResponse>();
  const [tableFilters, setTableFilters] = useState<Record<string, FilterValue | null>>();
  const [tablePaginationConfig, setTablePaginationConfig] = useState<TablePaginationConfig>();
  const [reloadTable, setReloadTable] = useState<boolean>(false);
  const [customerDetail, setCustomerDetail] = useState<IShopViewDetailDto>();
  const [currentShopId, setCurrentShopId] = useState('');

  const fetchShops = async (pageNumber: number | undefined = 1, pageSize: number | undefined = 10) => {
    const { success, data } = await apiGetShops(pageNumber, pageSize);

    if (success) {
      setPaginationResponse(data);
    }
  };

  const columns = [
    {
      title: 'STT',
      dataIndex: 'no',
      key: 'no',
      width: 50,
      align: 'center' as const,
    },
    {
      title: 'Mã KH riêng',
      dataIndex: 'shopUniqueCode',
      key: 'code',
      width: 120,
      render: (value: string, record: ShopDatatableDto) => {
        return (
          <Button
            type="link"
            onClick={() => {
              setCurrentShopId(record.id);
              handleViewDetail(record.id);
            }}
          >
            {value}
          </Button>
        );
      },
    },
    {
      title: 'Ngày đăng ký',
      dataIndex: 'registerDate',
      key: 'registerDate',
      width: 150,
      render: (value: string, record: ShopDatatableDto) => {
        const dateFormatted = dayjs(value).format(revertDateFormatMap['day']);
        return <span>{dateFormatted}</span>;
      },
    },
    {
      title: 'Tên cửa hàng',
      dataIndex: 'shopName',
      key: 'shopName',
    },
    {
      title: 'Tên khách hàng',
      dataIndex: 'fullName',
      key: 'fullName',
      width: 'auto',
    },
    {
      title: 'Email',
      dataIndex: 'email',
      key: 'email',
      width: 'auto',
    },
    {
      title: 'Sản lượng trung bình',
      dataIndex: 'avgMonthlyCapacity',
      key: 'avgMonthlyCapacity',
      width: 180,
      align: 'right' as const,
      render: (value: string) => {
        return <CommaDecimalDisplay value={value} />;
      },
    },
    {
      title: 'Kết nối đơn vị vận chuyển',
      dataIndex: 'totalDeliveryConnected',
      key: 'totalDeliveryConnected',
      align: 'right' as const,
      render: (value: any, record: ShopDatatableDto) => {
        if (record.isVerified) {
          return (
            <Button onClick={() => handleViewDetail(record.id)} type="link">
              {value}
            </Button>
          );
        } else {
          return '';
        }
      },
    },
    {
      title: 'Trạng thái',
      dataIndex: 'status',
      key: 'status',
      width: 120,
      align: 'center' as const,
      render: (value: any, record: ShopDatatableDto) => <Tag color={record['isVerified'] === true ? 'green' : 'orange'}>{value}</Tag>,
    },
    {
      title: 'Thao tác',
      key: 'action',
      width: 150,
      align: 'center' as const,
      render: (_: any, record: ShopDatatableDto) => {
        const { isVerified } = record;
        if (isVerified) {
          return null;
        }

        return (
          <div key={record.code}>
            <Button
              className="table-btn-action"
              size="small"
              style={{ marginRight: 5, backgroundColor: '#4CAF50', color: '#ffffff', borderColor: '#4CAF50' }}
              onClick={() => handleActivate(record)}
            >
              <CheckCircleOutlined /> Duyệt
            </Button>
          </div>
        );
      },
    },
  ];

  // Handle the Activate action
  const handleActivate = async (record: ShopDatatableDto) => {
    await apiActiveShops(record.id);
    setReloadTable(!reloadTable);
  };

  const handleChangeTable = (config: TablePaginationConfig, filters: Record<string, FilterValue | null>) => {
    const { current, pageSize } = config;

    setTablePaginationConfig(config);
    setTableFilters(filters);
    fetchShops(current, pageSize);
  };

  const handleViewDetail = async (id: string) => {
    const response = await apiGetShopDetail(id);
    if (response.success) {
      setCustomerDetail(response.data);
    }
  };

  const handleChangeAllowPublishOrder = async (payload: IChangeOperationConfig) => {
    const response = await apiChangeOperationConfig(payload);
    if (response.success) {
      setCustomerDetail(response.data);
    }
  };

  useEffect(() => {
    let _pageNumber: number | undefined = -1,
      _pageSize: number | undefined = -1,
      _supplierFilter: FilterValue | null = null;

    if (tablePaginationConfig) {
      const { current, pageSize } = tablePaginationConfig;

      _pageNumber = current;
      _pageSize = pageSize;
    }

    if (tableFilters) {
      _supplierFilter = tableFilters?.supplier;
    }

    if (Boolean(_pageSize) && (_pageSize as number) > 0 && Boolean(_pageNumber) && (_pageNumber as number) > 0) {
      fetchShops(_pageNumber, _pageSize);
    } else {
      fetchShops();
    }
  }, [tablePaginationConfig, tableFilters, reloadTable]);

  return (
    <Row>
      <Table
        style={{ width: '100%' }}
        columns={columns}
        dataSource={paginationResponse?.data as ShopDatatableDto[]}
        rowKey="key" // Unique key for rows
        scroll={{ x: 'max-content' }} // Enable horizontal scrolling for wide tables
        pagination={{
          pageSize: paginationResponse?.pageSize,
          current: paginationResponse?.pageNumber,
          total: paginationResponse?.count,
        }}
        onChange={handleChangeTable}
      />
      <CustomerDetail data={customerDetail} onChange={handleChangeAllowPublishOrder} callback={() => handleViewDetail(currentShopId)} />
    </Row>
  );
};

export default CustomerTable;
