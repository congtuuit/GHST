import type { IPaginationResponse } from '@/interface/business';
import type { TablePaginationConfig } from 'antd';
import type { FilterValue } from 'antd/es/table/interface';
import { Button, Form, Modal, Switch } from 'antd';
import { useEffect, useState } from 'react';
import { apiChangeAllowPublishOrder, apiGetDeliveryConfigs, apiUpdateDeliveryConfigs } from '@/api/business.api';
import Datatable from '@/components/core/datatable';
import { IDeliveryConfigDto } from '.';
import PartnerConfig from './PartnerConfig';
import ActionButton from '@/components/core/ActionButton';

interface TableDeliveryConfigProps {
  reload: boolean;
}
const TableDeliveryConfig = (props: TableDeliveryConfigProps) => {
  const { reload } = props;
  const [configs, setConfigs] = useState<IDeliveryConfigDto[]>([]);
  const [dataSource, setDataSource] = useState<IPaginationResponse<IDeliveryConfigDto> | null>(null);
  const [openEditDialog, setOpenEditDialog] = useState<boolean>(false);
  const [editData, setEditData] = useState<IDeliveryConfigDto | null>(null);
  const [form] = Form.useForm();

  const fetchConfigs = async () => {
    const response = await apiGetDeliveryConfigs();
    if (response.success) {
      setConfigs(response.data);
    }
  };

  const columns = [
    {
      title: 'STT',
      dataIndex: 'no',
      key: 'no',
      align: 'center' as const,
    },
    {
      title: 'Đơn vị giao hàng',
      dataIndex: 'deliveryPartnerName',
      key: 'deliveryPartnerName',
    },
    {
      title: 'API Key',
      dataIndex: 'apiKey',
      key: 'apiKey',
    },
    {
      title: 'Tài khoản',
      dataIndex: 'userName',
      key: 'userName',
    },
    {
      title: 'Trạng thái',
      dataIndex: 'isActivated',
      key: 'isActivated',
      width: 'auto',
      render: (value: boolean, record: IDeliveryConfigDto) => {
        return (
          <Switch
            onChange={active => handleToggelActiveConfig(active, record)}
            value={value}
            checkedChildren="Đang bật"
            unCheckedChildren="Đang tắt"
            size="default"
          />
        );
      },
    },
    {
      title: 'Thao tác',
      dataIndex: 'action',
      key: 'action',
      render: (value: any, record: IDeliveryConfigDto) => {
        return <ActionButton onClick={() => handleViewDetail(record)} iconAction="edit" text="Sửa" />;
      },
    },
  ];

  const handleToggelActiveConfig = async (active: boolean, record: IDeliveryConfigDto) => {
    record.isActivated = active;
    const response = await apiUpdateDeliveryConfigs([record]);
    if (response.success) {
      fetchConfigs();
    }
  };

  const handleUpdateConfig = async () => {
    const record = form.getFieldsValue();
    const response = await apiUpdateDeliveryConfigs([record]);
    if (response.success) {
      fetchConfigs();
      setOpenEditDialog(false);
    }
  };

  const handleChangeTable = (config: TablePaginationConfig, filters: Record<string, FilterValue | null>) => {
    const { current, pageSize } = config;

    // setTablePaginationConfig(config);
    // setTableFilters(filters);
    // fetchShops(current, pageSize);
  };

  const handleViewDetail = async (record: IDeliveryConfigDto) => {
    form.setFieldsValue(record);
    setOpenEditDialog(true);
  };

  const handleChangeAllowPublishOrder = async (id: string) => {
    const response = await apiChangeAllowPublishOrder(id);

    // if (response.success) {
    //   setCustomerDetail(response.data);
    // }
  };

  useEffect(() => {
    fetchConfigs();
  }, [reload]);

  useEffect(() => {
    const rs: IPaginationResponse<IDeliveryConfigDto> = {
      count: configs.length,
      data: configs?.map((i, index) => {
        return {
          ...i,
          no: index + 1,
        };
      }),
      pageNumber: 1,
      pageSize: 10,
    };

    setDataSource(rs);
  }, [configs]);

  return (
    <>
      <Datatable columns={columns} dataSource={dataSource} onChange={handleChangeTable} />;
      <Modal open={openEditDialog} title="Cấu hình" onCancel={() => setOpenEditDialog(false)} onOk={handleUpdateConfig}>
        <Form form={form} layout="vertical" style={{ width: '100%' }}>
          <PartnerConfig data={editData} />
        </Form>
      </Modal>
    </>
  );
};

export default TableDeliveryConfig;
