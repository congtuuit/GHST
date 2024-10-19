import type { ShopPricePlanDto } from '@/interface/business';

import { PlusOutlined } from '@ant-design/icons';
import { Button, Form, Input, InputNumber, message, Modal, Select } from 'antd';
import { useEffect, useState } from 'react';

const { Option } = Select;

interface PriceConfigurationFormProps {
  onSubmit: (values: ShopPricePlanDto, callback: any) => void;
  data: ShopPricePlanDto | undefined;
}

const PriceConfigurationForm = (props: PriceConfigurationFormProps) => {
  const { onSubmit, data } = props;
  const [isModalVisible, setIsModalVisible] = useState(false);
  const [processing, setProcessing] = useState(false);
  const [isEditing, setIsEditing] = useState(false);
  const [form] = Form.useForm(); // Create form instance

  useEffect(() => {
    // Điền dữ liệu vào form khi có object sẵn
    if (data) {
      setIsEditing(true);
      form.setFieldsValue({
        ...data,
      });

      showModal();
    }
  }, [data, form]);

  const showModal = () => {
    setIsModalVisible(true);
  };

  const handleOk = () => {
    form
      .validateFields()
      .then(values => {
        setProcessing(true);
        onSubmit &&
          onSubmit(values, (success: any) => {
            if (success === true) {
              setIsModalVisible(false);
              form.resetFields();
            } else {
              message.error('Thao tác thất bại, vui lòng kiểm tra lại!');
            }

            setProcessing(false);
          });
      })
      .catch(info => {
        console.log('Validation Failed:', info);
      });
  };

  const handleCancel = () => {
    setIsModalVisible(false);
    form.resetFields();
  };

  return (
    <>
      {/* Nút mở modal */}
      <Button type="primary" onClick={showModal} size="middle">
        <PlusOutlined /> Thêm cấu hình
      </Button>

      {/* Modal với form */}
      <Modal
        title="Thêm cấu hình bảng giá"
        open={isModalVisible}
        onOk={handleOk}
        onCancel={handleCancel}
        okText={isEditing ? 'Cập nhật' : 'Thêm'}
        cancelText="Hủy"
        confirmLoading={processing}
        maskClosable={false}
      >
        <Form layout="vertical" form={form}>
          <Form.Item hidden name="id">
            <Input />
          </Form.Item>

          {/* Dropdown Nhà cung cấp */}
          <Form.Item label="Nhà cung cấp" name="supplier" rules={[{ required: true, message: 'Vui lòng chọn nhà cung cấp!' }]}>
            <Select placeholder="Chọn nhà cung cấp">
              <Option value="GHN">GHN</Option>
              <Option value="SHOPEE EXPRESS">SHOPEE EXPRESS</Option>
              <Option value="J&T">J&T</Option>
              <Option value="Best">Best</Option>
              <Option value="Viettel">Viettel</Option>
              <Option value="GHTK">GHTK</Option>
            </Select>
          </Form.Item>

          {/* Giá riêng */}
          <Form.Item label="Giá riêng" name="privatePrice" rules={[{ required: true, message: 'Vui lòng nhập giá riêng!' }]}>
            <Input placeholder="Nhập giá riêng" type="number" />
          </Form.Item>

          {/* Giá công khai */}
          <Form.Item label="Giá công khai" name="officialPrice" rules={[{ required: true, message: 'Vui lòng nhập giá công khai!' }]}>
            <Input placeholder="Nhập giá công khai" type="number" />
          </Form.Item>

          {/* Sức chứa */}
          <Form.Item label="Khối lượng" name="capacity" rules={[{ required: true, message: 'Vui lòng nhập khối lượng!' }]}>
            <InputNumber placeholder="Nhập khối lượng" min={0} style={{ width: '100%' }} step="0.01" type="number" />
          </Form.Item>
        </Form>
      </Modal>
    </>
  );
};

export default PriceConfigurationForm;
