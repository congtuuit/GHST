import type { supplierKeys } from '@/constants/data';

import { Button, Card, Col, Form, Modal, Row } from 'antd';
import { useEffect, useState } from 'react';
import { apiGetDeliveryConfigs, apiUpdateDeliveryConfigs } from '@/api/business.api';
import { debounce } from '@/utils/common';
import PartnerConfig from './PartnerConfig';
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
