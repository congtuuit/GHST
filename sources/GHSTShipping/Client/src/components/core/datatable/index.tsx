import type { IPaginationResponse } from '@/interface/business';
import type { FilterValue } from 'antd/es/table/interface';
import type { TablePaginationConfig } from 'antd/lib/table';

import { Table } from 'antd';

interface DatatableProps<T> {
  columns: any[];
  dataSource: IPaginationResponse<T> | null;
  onChange?: (pagination: TablePaginationConfig, filters: Record<string, FilterValue | null>, sorter: any) => void; // Ensure proper typing for onChange
}

const Datatable = <T extends object>({ columns, dataSource, onChange }: DatatableProps<T>) => {
  return (
    <Table
      style={{ width: '100%' }}
      columns={columns}
      dataSource={dataSource?.data}
      rowKey={record => (record as any).id || (record as any).key} // Replace with the correct key field from your data
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
