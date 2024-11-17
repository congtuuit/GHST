import React, { forwardRef, useImperativeHandle, useState } from 'react';
import { Form, Input, InputNumber, Button, Switch, Space, Card, Row, Col, FormInstance } from 'antd';
import { DeliveryPricePlaneFormDto } from '@/interface/shop';



interface DeliveryPricePlaneFormProps {
  onSubmit: (values: DeliveryPricePlaneFormDto) => void;
  loading: boolean;
}

const DeliveryPricePlaneForm = forwardRef((props: DeliveryPricePlaneFormProps, ref) => {
  const { onSubmit, loading } = props;
  const [form] = Form.useForm<DeliveryPricePlaneFormDto>();
  const [isEdit, setIsEdit] = useState(false);

  const handleFinish = (values: DeliveryPricePlaneFormDto) => {
    onSubmit(values);
  };

  useImperativeHandle(
    ref,
    () => ({
      setValues: (values: Partial<DeliveryPricePlaneFormDto>) => {
        form.resetFields();
        form.setFieldsValue(values);
        if (values?.id) {
          setIsEdit(true);
        } else {
          setIsEdit(false);
        }
      },
      resetForm: () => {
        form.resetFields();
      },
    }),
    [form],
  );

  return (
    <Form
      form={form}
      layout="vertical"
      onFinish={handleFinish}
      initialValues={{
        IsActivated: true,
      }}
    >
      <Row gutter={24}>
        {/* Name */}

        <Form.Item hidden label="ID" name="id">
          <Input />
        </Form.Item>

        <Col span={24}>
          <Form.Item label="Tên Bảng Giá" name="name" rules={[{ required: true, message: 'Vui lòng nhập tên bảng giá' }]}>
            <Input placeholder="Nhập tên bảng giá" />
          </Form.Item>
        </Col>

        {/* MinWeight */}
        <Col span={12}>
          <Form.Item
            label="Trọng Lượng Tối Thiểu (gram)"
            name="minWeight"
            rules={[{ required: true, message: 'Vui lòng nhập trọng lượng tối thiểu' }]}
          >
            <InputNumber min={0} placeholder="Nhập trọng lượng tối thiểu" style={{ width: '100%' }} />
          </Form.Item>
        </Col>

        {/* MaxWeight */}
        <Col span={12}>
          <Form.Item label="Trọng Lượng Tối Đa (gram)" name="maxWeight" rules={[{ required: true, message: 'Vui lòng nhập trọng lượng tối đa' }]}>
            <InputNumber min={0} placeholder="Nhập trọng lượng tối đa" style={{ width: '100%' }} />
          </Form.Item>
        </Col>

        {/* PrivatePrice */}
        <Col span={12}>
          <Form.Item label="Giá Riêng (VND)" name="privatePrice" rules={[{ required: true, message: 'Vui lòng nhập giá riêng' }]}>
            <InputNumber min={0} placeholder="Nhập giá riêng" style={{ width: '100%' }} />
          </Form.Item>
        </Col>

        {/* PublicPrice */}
        <Col span={12}>
          <Form.Item label="Giá Công Khai (VND)" name="publicPrice" rules={[{ required: true, message: 'Vui lòng nhập giá công khai' }]}>
            <InputNumber min={0} placeholder="Nhập giá công khai" style={{ width: '100%' }} />
          </Form.Item>
        </Col>

        {/* StepWeight */}
        <Col span={12}>
          <Form.Item label="Trọng Lượng Tăng (gram)" name="stepWeight" rules={[{ required: true, message: 'Vui lòng nhập trọng lượng mỗi bước' }]}>
            <InputNumber min={0} placeholder="Nhập trọng lượng mỗi bước" style={{ width: '100%' }} />
          </Form.Item>
        </Col>

        {/* StepPrice */}
        <Col span={12}>
          <Form.Item
            label="Cước Trên Trọng Lượng Tăng (VND)"
            name="stepPrice"
            rules={[{ required: true, message: 'Vui lòng nhập giá tăng theo bước' }]}
          >
            <InputNumber min={0} placeholder="Nhập giá tăng theo bước" style={{ width: '100%' }} />
          </Form.Item>
        </Col>

        {/* LimitInsurance */}
        <Col span={12}>
          <Form.Item label="Giới Hạn Bảo Hiểm (VND)" name="limitInsurance" rules={[{ required: true, message: 'Vui lòng nhập giới hạn bảo hiểm' }]}>
            <InputNumber min={0} placeholder="Nhập giới hạn bảo hiểm" style={{ width: '100%' }} />
          </Form.Item>
        </Col>

        {/* InsuranceFeeRate */}
        <Col span={12}>
          <Form.Item label="Phí Bảo Hiểm (%)" name="insuranceFeeRate" rules={[{ required: true, message: 'Vui lòng nhập phí bảo hiểm' }]}>
            <InputNumber min={0} max={100} step={0.01} placeholder="Nhập phí bảo hiểm" style={{ width: '100%' }} />
          </Form.Item>
        </Col>

        {/* ReturnFeeRate */}
        <Col span={12}>
          <Form.Item label="Phí Hoàn Trả (%)" name="returnFeeRate" rules={[{ required: true, message: 'Vui lòng nhập phí hoàn trả' }]}>
            <InputNumber min={0} max={100} step={0.01} placeholder="Nhập phí hoàn trả" style={{ width: '100%' }} />
          </Form.Item>
        </Col>

        {/* ConvertWeightRate */}
        <Col span={12}>
          <Form.Item
            label="Tỷ Lệ Chuyển Đổi Trọng Lượng (gram)"
            name="convertWeightRate"
            rules={[{ required: true, message: 'Vui lòng nhập tỷ lệ chuyển đổi trọng lượng' }]}
          >
            <InputNumber min={0} placeholder="Nhập tỷ lệ chuyển đổi" style={{ width: '100%' }} />
          </Form.Item>
        </Col>

        {/* IsActivated */}
        <Col span={12}>
          <Form.Item hidden label="Kích Hoạt" name="isActivated" valuePropName="checked" initialValue={true}>
            <Switch />
          </Form.Item>
        </Col>
      </Row>

      {/* Submit Button */}
      <Form.Item>
        <Space>
          <Button type="primary" htmlType="submit" loading={loading}>
            {isEdit ? 'Cập nhật' : 'Tạo mới'}
          </Button>
        </Space>
      </Form.Item>
    </Form>
  );
});

export default DeliveryPricePlaneForm;
