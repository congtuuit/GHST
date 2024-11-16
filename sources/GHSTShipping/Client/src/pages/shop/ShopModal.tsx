// src/components/ShopModal.tsx
import React, { useEffect, useRef } from 'react';
import { Modal, Form, Input, message, Col, Row } from 'antd';
import { createShop, BasicShopInfoDto } from '@/features/shop';
import AddressComponent from '../orders/components/address.component';
import { useSelector } from 'react-redux';
import { shopIdSelector } from '@/stores/user.store';

interface ShopModalProps {
  visible: boolean;
  onClose: () => void;
  onSuccess: () => void;
  editData?: BasicShopInfoDto | null;
}

const ShopModal: React.FC<ShopModalProps> = ({ visible, onClose, onSuccess, editData }) => {
  const [form] = Form.useForm<BasicShopInfoDto>();
  const addressRef = useRef<any>(null);
  const shopId = useSelector(shopIdSelector);

  useEffect(() => {
    if (editData) {
      form.setFieldsValue(editData);
      setTimeout(() => {
        addressRef.current?.update(editData);
      }, 300);
    } else {
      form.resetFields();
    }
  }, [editData, form]);

  const handleSubmit = async (values: BasicShopInfoDto) => {
    try {
      const response = await createShop(values);
      if (response.success) {
        message.success(editData ? 'Cập nhật thành công' : 'Tạo mới thành công');
        onSuccess();
        onClose();
      }
    } catch (error) {
      message.error('Lỗi khi lưu dữ liệu');
    }
  };

  return (
    <Modal
      title={editData ? 'Chỉnh sửa cửa hàng' : 'Thêm mới cửa hàng'}
      open={visible}
      onCancel={onClose}
      onOk={() => form.submit()}
      width={800}
      okText={editData ? 'Cập nhật' : 'Thêm mới'}
    >
      <Form form={form} layout="vertical" onFinish={handleSubmit}>
        <Row gutter={[16, 16]}>
          <Form.Item name="shopId" hidden initialValue={shopId}>
            <Input />
          </Form.Item>

          <Form.Item name="id" hidden>
            <Input />
          </Form.Item>

          <Col span={12}>
            <Form.Item label="Tên cửa hàng" name="name" rules={[{ required: true, message: 'Vui lòng nhập tên cửa hàng' }]}>
              <Input placeholder="Nhập tên cửa hàng" />
            </Form.Item>

            <Form.Item label="Điện thoại" name="phoneNumber" rules={[{ required: true, message: 'Vui lòng nhập số điện thoại' }]}>
              <Input placeholder="Nhập số điện thoại" />
            </Form.Item>
          </Col>
          <Col span={12}>
            <AddressComponent
              ref={addressRef}
              key={'address'}
              required={true}
              form={form}
              returnField={{
                address: 'address',
                districtId: 'districtId',
                districtName: 'districtName',
                wardId: 'wardId',
                wardName: 'wardName',
                provinceId: 'provinceId',
                provinceName: 'provinceName',
              }}
            />
          </Col>
        </Row>
      </Form>
    </Modal>
  );
};

export default ShopModal;
