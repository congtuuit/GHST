import type { supplierKeys } from '@/constants/data';
import { Button, Modal } from 'antd';
import { useEffect, useState } from 'react';
import CreatePartnerConfigForm from './CreatePartnerConfigForm';
import TableDeliveryConfig from './TableDeliveryConfig';
import { PlusOutlined } from '@ant-design/icons';

export interface IDeliveryConfigDto {
  no: number;
  id: string;
  apiKey: string;
  userName: string;
  isActivated: boolean;
  deliveryPartnerName: typeof supplierKeys;
  fullName: string;
  email: string;
  phoneNumber: string;
}

const DeliveryConfigPage = () => {
  const [reloadTable, setReloadTable] = useState<boolean>(false);
  const [openCreateForm, setOpenCreateForm] = useState<boolean>(false);

  const handleOpenCreateForm = () => {
    setOpenCreateForm(true);
  };

  const handleFinishCreateForm = () => {
    setOpenCreateForm(false);
    setReloadTable(!reloadTable);
  };

  useEffect(() => {}, []);

  return (
    <div>
      <Button type="primary" style={{ marginBottom: '20px' }} onClick={handleOpenCreateForm}>
        <PlusOutlined /> Thêm cấu hình
      </Button>
      <Modal open={openCreateForm} closeIcon={false} title="Thêm cấu hình" footer={null}>
        <CreatePartnerConfigForm onFinish={handleFinishCreateForm} />
      </Modal>

      <TableDeliveryConfig reload={reloadTable} />
    </div>
  );
};

export default DeliveryConfigPage;
