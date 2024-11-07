import type { IPaginationResponse, PaginationResponse, ShopPricePlanDto } from '@/interface/business';
import type { TablePaginationConfig } from 'antd';
import type { FilterValue } from 'antd/es/table/interface';

import { DeleteOutlined, EditOutlined } from '@ant-design/icons';
import { Button, message, Modal, Row, Table, Tag, Typography } from 'antd';
import { type FC, useEffect, useState } from 'react';

import { apiDeleteShopPricePlan, apiGetShopPricePlanes } from '@/api/business.api';
import { suppliers } from '@/constants/data';
import Datatable from '@/components/core/datatable';

const { confirm } = Modal;

interface ShopPricePlanDatatable {
  no: number;
  id: string; // Guid in C# can be represented as a string in TypeScript
  shopId: string; // Guid in C# can be represented as a string in TypeScript
  shopName: string;
  shopUniqueCode: string;
  supplier: 'GHN' | 'SHOPEE EXPRESS' | 'J&T' | 'Best' | 'Viettel' | 'GHTK'; // Có thể giới hạn giá trị nếu cần
  privatePrice: number; // decimal in C# can be represented as number in TypeScript
  officialPrice: number; // decimal in C# can be represented as number in TypeScript
  weight: number;
  length: number;
  width: number;
  height: number;
  convertedWeight: number;
}

interface PriceTableProps {
  selectedShop: string | undefined;
  refreshTable: boolean;
  onEdit: (value: ShopPricePlanDto, callback: (success: boolean) => void) => void;
}

const PriceTable = (props: PriceTableProps) => {
  const { selectedShop, refreshTable, onEdit } = props;
  const [reloadTable, setReloadTable] = useState<boolean>(refreshTable);
  const [pagination, setPagination] = useState<IPaginationResponse<ShopPricePlanDatatable> | null>(null);
  const [tableFilters, setTableFilters] = useState<Record<string, FilterValue | null>>();
  const [tablePaginationConfig, setTablePaginationConfig] = useState<TablePaginationConfig>();

  const fetchPricePlanes = async (
    shopId: string | undefined,
    supplier: any,
    pageNumber: number | undefined = 1,
    pageSize: number | undefined = 10,
  ) => {
    if (!Boolean(shopId)) {
      return;
    }

    let supplierQuery = '';
    if (Boolean(supplier)) {
      supplierQuery = supplier?.length > 0 ? supplier.join(',') : ''; // join the array into a comma-separated string
    }

    const { success, data } = await apiGetShopPricePlanes(shopId, supplierQuery, pageNumber, pageSize);
    if (success) {
      setPagination(data);
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
      key: 'shopUniqueCode',
      width: 120,
      align: 'left' as const,
    },
    {
      title: 'Cửa hàng',
      dataIndex: 'shopName',
      key: 'shopName',
      align: 'left' as const,
      width: 180,
    },
    {
      title: 'Đơn vị vận chuyển',
      dataIndex: 'supplier',
      key: 'supplier',
      width: 180,
      align: 'center' as const,
      filters: suppliers.map(i => {
        return {
          text: i,
          value: i,
        };
      }),
    },
    {
      title: 'Giá riêng',
      dataIndex: 'privatePrice',
      key: 'privatePrice',
      align: 'right' as const,
      width: 120,
      render: (value: any, record: ShopPricePlanDatatable) => {
        return (
          <Typography.Text type="secondary">
            {new Intl.NumberFormat('vi-VN', {
              style: 'currency',
              currency: 'VND',
            }).format(value)}
          </Typography.Text>
        );
      },
    },
    {
      title: 'Giá công khai',
      dataIndex: 'officialPrice',
      key: 'officialPrice',
      align: 'right' as const,
      width: 120,
      render: (value: any, record: ShopPricePlanDatatable) => {
        return (
          <Typography.Text type="success">
            {new Intl.NumberFormat('vi-VN', {
              style: 'currency',
              currency: 'VND',
            }).format(value)}
          </Typography.Text>
        );
      },
    },
    {
      title: 'Cân nặng',
      dataIndex: 'weight',
      key: 'weight',
      align: 'right' as const,
    },
    {
      title: 'Dài x Rộng x Cao',
      dataIndex: '_lxwxh',
      key: '_lxwxh',
      align: 'right' as const,
      render: (value: string, record: ShopPricePlanDatatable) => {
        return <span>{`${record.length}x${record.width}x${record.height}`}</span>;
      },
    },
    {
      title: 'Khối lượng chuyển đổi',
      dataIndex: 'convertedWeight',
      key: 'convertedWeight',
      align: 'right' as const,
    },
    {
      title: 'Thao tác',
      key: 'action',
      width: 200,
      align: 'center' as const,
      render: (_: any, record: ShopPricePlanDatatable) => {
        return (
          <div key={record.id}>
            <Button className="table-btn-action" size="small" onClick={() => handleEdit(record)}>
              <EditOutlined /> Sửa
            </Button>
            <Button danger className="table-btn-action" size="small" onClick={() => showDeleteConfirm(record)}>
              <DeleteOutlined />
            </Button>
          </div>
        );
      },
    },
  ];

  const showDeleteConfirm = (record: ShopPricePlanDatatable) => {
    confirm({
      title: 'Bạn có chắc chắn muốn xóa mục này?',
      okText: 'Đồng ý',
      okType: 'danger',
      cancelText: 'Hủy',
      centered: true,
      onOk() {
        handleDelete(record);
      },
      onCancel() {},
    });
  };

  // Handle the Activate action
  const handleEdit = async (record: ShopPricePlanDatatable) => {
    onEdit &&
      onEdit(
        {
          id: record.id,
          shopId: record.shopId,
          supplier: record.supplier,
          privatePrice: record.privatePrice,
          officialPrice: record.officialPrice,
          weight: record.weight,
          length: record.length,
          width: record.width,
          height: record.height,
          convertedWeight: record.convertedWeight,
        } as ShopPricePlanDto,
        (success: boolean) => {
          if (success) {
            setReloadTable(!reloadTable);
          }
        },
      );
  };

  const handleDelete = async (record: ShopPricePlanDatatable) => {
    const { id } = record;
    await apiDeleteShopPricePlan(id);
    setReloadTable(!reloadTable);
  };

  const handleChangeTable = (config: TablePaginationConfig, filters: Record<string, FilterValue | null>) => {
    setTablePaginationConfig(config);
    setTableFilters(filters);
  };

  const handleSelectedRows = (rows: ShopPricePlanDatatable[]) => {
  };

  const handleDeleteRows = async (rows: ShopPricePlanDatatable[]) => {
    const ids = rows.map(i => i.id);
    const response = await apiDeleteShopPricePlan(ids[0], ids);
    if (response.success) {
      message.success('Xoá thành công!');
      setReloadTable(!reloadTable);
    }
  };

  useEffect(() => {
    if (!Boolean(selectedShop)) {
      return;
    }

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

    if (Boolean(_pageSize) && (_pageSize as number) > 0 && Boolean(_pageNumber) && (_pageNumber as number) > 0 && Boolean(_supplierFilter)) {
      fetchPricePlanes(selectedShop, _supplierFilter, _pageNumber, _pageSize);
    } else if (Boolean(_pageSize) && (_pageSize as number) > 0 && Boolean(_pageNumber) && (_pageNumber as number) > 0) {
      fetchPricePlanes(selectedShop, '', _pageNumber, _pageSize);
    } else {
      fetchPricePlanes(selectedShop, '');
    }
  }, [tablePaginationConfig, tableFilters, reloadTable, selectedShop]);

  useEffect(() => {
    setReloadTable(!reloadTable);
  }, [refreshTable]);

  return (
    <Datatable
      columns={columns}
      dataSource={pagination}
      onChange={handleChangeTable}
      onSelectedRows={handleSelectedRows}
      handleDeleteRows={handleDeleteRows}
      mode="mutilple"
    />
  );
};

export default PriceTable;
