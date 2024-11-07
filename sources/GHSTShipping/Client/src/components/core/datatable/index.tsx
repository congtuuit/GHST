import type { IPaginationResponse } from '@/interface/business';
import type { FilterValue, TableRowSelection } from 'antd/es/table/interface';
import type { TablePaginationConfig } from 'antd/lib/table';
import { Table, Input, Col, Button, Row } from 'antd';
import { ReactElement, useEffect, useState } from 'react';
import { DeleteOutlined } from '@ant-design/icons';

const { Search } = Input;

interface DatatableProps<T> {
  columns: any[];
  dataSource: IPaginationResponse<T> | null;
  onChange?: (pagination: TablePaginationConfig, filters: Record<string, FilterValue | null>, sorter: any) => void; // Ensure proper typing for onChange
  onSearch?: () => void;
  showSearch?: boolean;
  rowSelection?: TableRowSelection<T>;
  onSelectedRows?: (selectedRows: T[]) => void;
  handleDeleteRows?: (selectedRows: T[]) => void;
  mode?: 'single' | 'mutilple';
  headerBox?: ReactElement;
  loading?: boolean;
}

const Datatable = <T extends object>({
  columns,
  dataSource,
  onChange,
  onSearch,
  onSelectedRows,
  showSearch,
  handleDeleteRows,
  mode = 'single',
  headerBox,
  loading,
}: DatatableProps<T>) => {
  const [selectionType, setSelectionType] = useState<'checkbox' | 'radio'>('checkbox');
  const [selectedRows, setSelectedRows] = useState<T[]>([]);

  const handleSelectedRows = (rows: T[]) => {
    setSelectedRows(rows);
    onSelectedRows && onSelectedRows(rows);
  };

  useEffect(() => {
    setSelectedRows([]);
  }, [dataSource]);

  return (
    <div style={{ width: '100%' }}>
      {showSearch || headerBox ? (
        <Row>
          {headerBox && headerBox}
          {showSearch && (
            <Col span={8}>
              <Search placeholder="Nhập để tìm kiếm" onSearch={onSearch} enterButton />
            </Col>
          )}
        </Row>
      ) : (
        <></>
      )}

      {mode === 'mutilple' && selectedRows.length > 0 && (
        <Button style={{ marginTop: '20px', marginBottom: '10px' }} danger onClick={() => handleDeleteRows && handleDeleteRows(selectedRows ?? [])}>
          <DeleteOutlined /> {`Xóa ${selectedRows.length} mục`}
        </Button>
      )}

      {mode === 'single' && (
        <Table
          loading={loading}
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
      )}

      {mode === 'mutilple' && (
        <Table
          loading={loading}
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
          rowSelection={{
            type: selectionType,
            onChange(_, selectedRows) {
              handleSelectedRows(selectedRows);
            },
          }}
        />
      )}
    </div>
  );
};

export default Datatable;
