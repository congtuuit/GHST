import { Table } from 'antd';
import { IPaginationResponse } from '@/interface/business';

interface DatatableProps<T> {
  columns: any[];
  dataSource: IPaginationResponse<T> | null;
  onChange: (pagination: any, filters: any, sorter: any) => void; // Ensure proper typing for onChange
}

const Datatable = <T extends object>({ columns, dataSource, onChange }: DatatableProps<T>) => {
  return (
    <Table
      style={{ width: '100%' }}
      columns={columns}
      dataSource={dataSource?.data}
      rowKey={(record) => (record as any).id || (record as any).key} // Replace with the correct key field from your data
      scroll={{ x: 'max-content' }}
      pagination={{
        pageSize: dataSource?.pageSize || 10, // Default value if undefined
        current: dataSource?.pageNumber || 1, // Default value if undefined
        total: dataSource?.count || 0, // Default value if undefined
      }}
      onChange={onChange}
    />
  );
};

export default Datatable;
