import { apiGetDeliveryConfigs, apiUpdateDeliveryConfigs } from '@/api/business.api';
import { supplierKeys } from '@/constants/data';
import { Button, Card, Col, Form, Row } from 'antd';
import { useEffect, useState } from 'react';
import PartnerConfig from './PartnerConfig';
import { SaveOutlined } from '@ant-design/icons';
import { debounce } from '@/utils/common';

export interface IDeliveryConfigDto {
  id: string;
  apiKey: string;
  userName: string;
  isActivated: boolean;
  deliveryPartnerName: typeof supplierKeys;
}

const DeliveryConfigPage = () => {
  const [configs, setConfigs] = useState<IDeliveryConfigDto[]>([]);
  const [form] = Form.useForm();
  const [formChanged, setFormChanged] = useState<boolean>(false);
  const [isLoading, setIsLoading] = useState<boolean>(false);

  const fetchConfigs = async () => {
    const response = await apiGetDeliveryConfigs();
    if (response.success) {
      setConfigs(response.data);
    }
  };

  const saveConfigs = async () => {
    const values = form.getFieldsValue();
    const { deliveryConfigs } = values;
    const response = await apiUpdateDeliveryConfigs(deliveryConfigs);
    if (response.success) {
      fetchConfigs();
      setFormChanged(false);
    }
  };

  const onFinish = async (values: any) => {
    setIsLoading(true);
    await saveConfigs();
  };

  const handleFormChange = async (e: any) => {
    await saveConfigs();
  };

  useEffect(() => {
    fetchConfigs();
  }, []);

  useEffect(() => {
    setIsLoading(false);
  }, [configs]);

  return (
    <Form form={form} onFinish={onFinish} onFieldsChange={debounce(handleFormChange, 1000)}>
      <Row>
        <Col span={24}>
          {/* <Button type="primary" htmlType="submit" disabled={!formChanged} loading={isLoading}>
            <SaveOutlined /> Lưu
          </Button> */}
          {configs?.map((data, index) => {
            return (
              <Card title={data.deliveryPartnerName} key={index}>
                <PartnerConfig data={data} index={index} />
              </Card>
            );
          })}
        </Col>
      </Row>
    </Form>
  );
};

export default DeliveryConfigPage;
