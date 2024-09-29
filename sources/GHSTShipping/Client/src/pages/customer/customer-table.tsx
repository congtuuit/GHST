import { useEffect, useState, type FC } from 'react';
import { Button, Table, TablePaginationConfig, Tag } from 'antd';
import { CheckCircleOutlined } from '@ant-design/icons';
import { apiActiveShops, apiGetShops } from '@/api/user.api';
import { PaginationResponse } from '@/interface/business';
import { FilterValue } from 'antd/es/table/interface';

interface ShopDatatableDto {
  key: string;
  id: string;
  code: string;
  registerDate: string;
  shopName: string;
  fullName: string;
  avgMonthlyCapacity: number;
  status: string;
  isVerified: boolean;
}

const CustomerTable: FC = () => {
  const [paginationResponse, setPaginationResponse] = useState<PaginationResponse>();
  const [tableFilters, setTableFilters] = useState<Record<string, FilterValue | null>>();
  const [tablePaginationConfig, setTablePaginationConfig] = useState<TablePaginationConfig>();
  const [reloadTable, setReloadTable] = useState<Boolean>(false);

  // const getShops = async () => {
  //   const { success, data } = await apiGetShops();
  //   if (success) {
  //     setPaginationResponse(data);
  //   }
  // };

  const fetchShops = async (
    pageNumber: number | undefined = 1,
    pageSize: number | undefined = 10,
  ) => {
    const { success, data } = await apiGetShops(pageNumber, pageSize);
    if (success) {
      setPaginationResponse(data);
    }
  };

  const columns = [
    {
      title: 'No',
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
    },
    {
      title: 'Ngày đăng ký',
      dataIndex: 'registerDate',
      key: 'registerDate',
      width: 150,
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
      title: 'Sản lượng trung bình',
      dataIndex: 'avgMonthlyCapacity',
      key: 'avgMonthlyCapacity',
      width: 180,
      align: 'right' as const,
    },
    {
      title: 'Trạng thái',
      dataIndex: 'status',
      key: 'status',
      width: 120,
      align: 'center' as const,
      render: (value: any, record: ShopDatatableDto) => (
        <Tag color={record['isVerified'] === true ? 'green' : 'orange'}>{value}</Tag>
      ),
    },
    {
      title: 'Thao tác',
      key: 'action',
      width: 100,
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


  useEffect(() => {
    fetchShops();
  }, []);

  return (
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
  );
};

export default CustomerTable;