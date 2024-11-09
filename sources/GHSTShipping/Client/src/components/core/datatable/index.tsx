import type { IPaginationResponse } from '@/interface/business';
import type { FilterValue, TableRowSelection } from 'antd/es/table/interface';
import type { TablePaginationConfig } from 'antd/lib/table';
import { Table, Input, Col, Button, Row } from 'antd';
import { ReactElement, useEffect, useState, useImperativeHandle, forwardRef } from 'react';
import { DeleteOutlined } from '@ant-design/icons';

const { Search } = Input;

interface DatatableProps<T> {
  columns: any[];
  dataSource: IPaginationResponse<T> | null;
  onChange?: (pagination: TablePaginationConfig, filters: Record<string, FilterValue | null>, sorter: any) => void;
  onSearch?: (value: string) => void;
  showSearch?: boolean;
  rowSelection?: TableRowSelection<T>;
  onSelectedRows?: (selectedRows: T[]) => void;
  handleDeleteRows?: (selectedRows: T[]) => void;
  mode?: 'single' | 'multiple';
  headerBox?: ReactElement;
  loading?: boolean;
  ref?: React.Ref<DatatableRef>;
}

export interface DatatableRef {
  clearSelectedRows: () => void;
}

const Datatable = forwardRef(
  <T extends object>(
    {
      columns,
      dataSource,
      onChange,
      onSearch,
      onSelectedRows,
      showSearch = false,
      handleDeleteRows,
      mode = 'single',
      headerBox,
      loading = false,
    }: DatatableProps<T>,
    ref: React.Ref<DatatableRef>,
  ) => {
    const [selectedRows, setSelectedRows] = useState<T[]>([]);

    const handleSelectedRows = (rows: T[]) => {
      setSelectedRows(rows);
      onSelectedRows?.(rows);
    };

    // Hàm để xóa các hàng đã chọn
    const clearSelectedRows = () => {
      setSelectedRows([]);
    };

    // Expose clearSelectedRows method using forwardRef
    useImperativeHandle(ref, () => ({
      clearSelectedRows,
    }));

    useEffect(() => {
      setSelectedRows([]);
    }, [dataSource]);

    const renderSearchBox = () => (
      <Row gutter={16}>
        {headerBox && <Col>{headerBox}</Col>}
        {showSearch && (
          <Col span={24}>
            <Col span={8}>
              <Search placeholder="Nhập để tìm kiếm" onChange={e => onSearch && onSearch(e.target.value)} onSearch={onSearch} enterButton />
            </Col>
          </Col>
        )}
      </Row>
    );

    const renderDeleteButton = () =>
      mode === 'multiple' &&
      selectedRows.length > 0 &&
      handleDeleteRows && (
        <Button style={{ marginTop: 20, marginBottom: 10 }} danger onClick={() => handleDeleteRows(selectedRows)}>
          <DeleteOutlined /> {`Xóa ${selectedRows.length} mục`}
        </Button>
      );

    const tablePagination = {
      pageSize: dataSource?.pageSize || 10,
      current: dataSource?.pageNumber || 1,
      total: dataSource?.count || 0,
    };

    const renderTable = () => (
      <Table
        loading={loading}
        columns={columns}
        dataSource={dataSource?.data}
        rowKey={record => (record as any).id || (record as any).key}
        scroll={{ x: 'max-content' }}
        pagination={tablePagination}
        onChange={onChange}
        rowSelection={
          mode === 'multiple'
            ? {
                type: 'checkbox',
                onChange: (_, selectedRows) => handleSelectedRows(selectedRows),
              }
            : undefined
        }
      />
    );

    return (
      <div style={{ width: '100%' }}>
        {(showSearch || headerBox) && renderSearchBox()}
        {renderDeleteButton()}
        {renderTable()}
      </div>
    );
  },
);

export default Datatable;
