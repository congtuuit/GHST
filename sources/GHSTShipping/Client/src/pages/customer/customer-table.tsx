import { useEffect, useState, type FC } from 'react';
import { Button, Table, Tag } from 'antd';
import { CheckCircleOutlined } from '@ant-design/icons';
import { apiActiveShops, apiGetShops } from '@/api/user.api';

interface ColumnType {
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
  const [dataSource, setDataSource] = useState<ColumnType[]>([]);

  const getShops = async () => {
    const { success, data } = await apiGetShops();
    if (success) {
      setDataSource(data?.data?.map(i => mappingToDatatable(i)));
    }
  };

  const mappingToDatatable = (input: any) => {
    return {
      ...input,
      loading: false, // Initialize loading state to false
    };
  };

  const refreshData = () => {
    getShops(); // Call getShops to refresh the data
  };

  const columns = [
    {
      title: 'No',
      dataIndex: 'no',
      key: 'no',
      width: 50,
    },
    {
      title: 'Code',
      dataIndex: 'shopUniqueCode',
      key: 'code',
      width: 120,
    },
    {
      title: 'Register Date',
      dataIndex: 'registerDate',
      key: 'registerDate',
      width: 150,
    },
    {
      title: 'Shop Name',
      dataIndex: 'shopName',
      key: 'shopName',
    },
    {
      title: 'Full Name',
      dataIndex: 'fullName',
      key: 'fullName',
      width: 'auto',
    },
    {
      title: 'Avg Monthly Capacity',
      dataIndex: 'avgMonthlyCapacity',
      key: 'avgMonthlyCapacity',
      width: 180,
    },
    {
      title: 'Status',
      dataIndex: 'status',
      key: 'status',
      width: 120,
      render: (value: any, record: ColumnType) => (
        <Tag color={record['isVerified'] === true ? 'green' : 'orange'}>{value}</Tag>
      ),
    },
    {
      title: 'Action',
      key: 'action',
      width: 100,
      render: (_: any, record: ColumnType) => {
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
  const handleActivate = async (record: ColumnType) => {

    // Call API to activate the shop
    await apiActiveShops(record.id);

    refreshData();
  };

  useEffect(() => {
    getShops();
  }, []);

  return (
    <Table
      style={{ width: '100%' }}
      columns={columns}
      dataSource={dataSource}
      pagination={{ pageSize: 10 }} // Optional: Set pagination size
      rowKey="key" // Unique key for rows
      scroll={{ x: 'max-content' }} // Enable horizontal scrolling for wide tables
    />
  );
};

export default CustomerTable;
