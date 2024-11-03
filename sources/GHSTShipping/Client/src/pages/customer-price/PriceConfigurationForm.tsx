import type { ShopPricePlanDto } from '@/interface/business';

import { PlusOutlined } from '@ant-design/icons';
import { Button, Col, Form, Input, InputNumber, message, Modal, Radio, Row, Select } from 'antd';
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
  const [priceConfigMode, setPriceConfigMode] = useState('single');
  const [form] = Form.useForm(); // Create form instance
  const [currentConvertedWeight, setCurrentConvertedWeight] = useState<number>(0);

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
        const { mode, officialMaxPrice, officialPrice, stepPrice, stepWeight } = values;
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

  const handleFormChange = () => {
    const formValues = form.getFieldsValue();
    const { mode, length, width, height } = formValues;
    if (mode === 'mutilple') {
      const _convertedWeight = (length ?? 0) * (width ?? 0) * (height ?? 0);
      setCurrentConvertedWeight(_convertedWeight);
    }
  };

  return (
    <>
      {/* Nút mở modal */}
      <Button type="primary" onClick={showModal} size="middle">
        <PlusOutlined /> Thêm cấu hình
      </Button>

      {/* Modal với form */}
      <Modal
        centered
        title="Thêm cấu hình bảng giá"
        open={isModalVisible}
        onOk={handleOk}
        onCancel={handleCancel}
        okText={isEditing ? 'Cập nhật' : 'Thêm'}
        cancelText="Hủy"
        confirmLoading={processing}
        maskClosable={false}
      >
        <Form layout="vertical" form={form} onFieldsChange={handleFormChange}>
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

          <Row gutter={[16, 0]}>
            <Col span={12}>
              {/* Sức chứa */}
              <Form.Item label="Khối lượng" name="weight" rules={[{ required: true, message: 'Vui lòng nhập khối lượng!' }]}>
                <InputNumber placeholder="Nhập khối lượng" min={0} style={{ width: '100%' }} step="1" type="number" />
              </Form.Item>
            </Col>
            <Col span={12}>
              <Form.Item label="Dài" name="length" rules={[{ required: true, message: 'Vui lòng nhập!' }]}>
                <InputNumber placeholder="Nhập giá trị" min={0} style={{ width: '100%' }} step="1" type="number" />
              </Form.Item>
            </Col>
            <Col span={12}>
              <Form.Item label="Rộng" name="width" rules={[{ required: true, message: 'Vui lòng nhập!' }]}>
                <InputNumber placeholder="Nhập giá trị" min={0} style={{ width: '100%' }} step="1" type="number" />
              </Form.Item>
            </Col>
            <Col span={12}>
              <Form.Item label="Cao" name="height" rules={[{ required: true, message: 'Vui lòng nhập!' }]}>
                <InputNumber placeholder="Nhập giá trị" min={0} style={{ width: '100%' }} step="1" type="number" />
              </Form.Item>
            </Col>

            <Col span={24}>
              {/* Giá riêng */}
              <Form.Item label="Giá riêng" name="privatePrice" rules={[{ required: true, message: 'Vui lòng nhập giá riêng!' }]}>
                <Input placeholder="Nhập giá riêng" type="number" />
              </Form.Item>
            </Col>

            <Col span={24}>
              <Radio.Group
                defaultValue="single"
                onChange={e => {
                  setPriceConfigMode(e.target.value);
                  form.setFieldValue('mode', e.target.value);
                  handleFormChange();
                }}
              >
                <Radio value="single">Giá đơn</Radio>
                <Radio value="mutilple">Tự sinh</Radio>
              </Radio.Group>
            </Col>

            <Form.Item hidden name="mode" initialValue={priceConfigMode}>
              <Input />
            </Form.Item>

            <Col span={24}>
              {priceConfigMode === 'single' && (
                <Form.Item label="Giá công khai" name="officialPrice" rules={[{ required: true, message: 'Vui lòng nhập giá công khai!' }]}>
                  <Input placeholder="Nhập giá công khai" type="number" />
                </Form.Item>
              )}

              {priceConfigMode === 'mutilple' && (
                <>
                  <Form.Item label="Giá khởi điểm" name="officialPrice" rules={[{ required: true, message: 'Vui lòng nhập' }]}>
                    <Input placeholder="Nhập giá trị" type="number" />
                  </Form.Item>
                  <p style={{ fontStyle: 'italic' }}>Giá trị khối lượng chuyển đổi hiện tại: {currentConvertedWeight ?? 0}</p>
                  <Form.Item label="Giá trị khối lượng tối đa" name="maxConvertedWeight" rules={[{ required: true, message: 'Vui lòng nhập' }]}>
                    <Input placeholder="Nhập giá trị" type="number" />
                  </Form.Item>
                  <Form.Item label="Bước nhảy của khối lượng chuyển đổi" name="stepWeight" rules={[{ required: true, message: 'Vui lòng nhập!' }]}>
                    <Input placeholder="Nhập giá trị" type="number" />
                  </Form.Item>
                  <Form.Item label="Bước nhảy giá" name="stepPrice" rules={[{ required: true, message: 'Vui lòng nhập!' }]}>
                    <Input placeholder="Nhập giá trị" type="number" />
                  </Form.Item>
                </>
              )}
            </Col>
          </Row>
        </Form>
      </Modal>
    </>
  );
};

export default PriceConfigurationForm;
