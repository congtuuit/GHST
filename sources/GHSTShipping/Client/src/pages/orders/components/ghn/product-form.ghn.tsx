import React, { useState } from 'react';
import { Form, Input, Button, Row, Col, Upload, Card, message, InputNumber } from 'antd';
import { PlusOutlined, UploadOutlined, DeleteOutlined } from '@ant-design/icons';
import MyInputNumber from '../MyInputNumber';

interface IProduct {
  name: string;
  code: string;
  weight: string;
  quantity: string;
}

const ProductForm = () => {
  const [products, setProducts] = useState<IProduct[]>([{ name: '', code: '', weight: '200', quantity: '1' }]);

  const addProduct = () => {
    setProducts([...products, { name: '', code: '', weight: '1', quantity: '1' }]);
  };

  const handleInputChange = (index: number, field: keyof IProduct, value: string) => {
    const newProducts: IProduct[] = [...products];
    if (newProducts[index]) {
      newProducts[index][field] = value;
    }

    setProducts(newProducts);
  };

  const handleRemoveProduct = (index: number) => {
    if (products.length === 1) {
      message.info('Đơn hàng phải có ít nhất 1 sản phẩm');
      return;
    }
    products.splice(index, 1);
    const newProducts: IProduct[] = [...products];
    setProducts(newProducts);
  };

  return (
    <Card title="Thông tin sản phẩm" style={{ marginBottom: '16px' }}>
      {products.map((product, index: number) => (
        <div key={index} style={{ marginBottom: 16, border: '1px solid #e8e8e8', padding: 16 }}>
          <Row gutter={[16, 16]}>
            {/* Product Name */}
            <Col span={4}>
              <Form.Item label={`SP ${index + 1}`} name={['items', index, 'name']}>
                <Input
                  placeholder="Nhập tên sản phẩm"
                  value={product.name}
                  onChange={e => handleInputChange(index, 'name', e.target.value)}
                />
              </Form.Item>
            </Col>

            {/* Weight */}
            <Col span={4}>
              <Form.Item label="KL (gram)" name={['items', index, 'weight']}>
                <MyInputNumber value={product.weight} onChange={e => handleInputChange(index, 'weight', e || '')} />
              </Form.Item>
            </Col>

            {/* Quantity */}
            <Col span={4}>
              <Form.Item label="Số lượng" name={['items', index, 'quantity']}>
                <MyInputNumber value={product.quantity} onChange={e => handleInputChange(index, 'quantity', e || '')} />
              </Form.Item>
            </Col>

            {/* Product Code */}
            <Col span={4}>
              <Form.Item label="Mã sản phẩm" name={['items', index, 'code']}>
                <Input
                  placeholder="Nhập mã sản phẩm"
                  value={product.code}
                  onChange={e => handleInputChange(index, 'code', e.target.value)}
                />
              </Form.Item>
            </Col>

            {/* Image Upload */}
            <Col span={4}>
              <Form.Item label="Hình ảnh">
                <Upload>
                  <Button icon={<UploadOutlined />}>Upload</Button>
                </Upload>
              </Form.Item>
            </Col>
          </Row>

          <Row justify={'end'}>
            {products.length > 1 && (
              <Button danger type="link" title="Xóa" onClick={() => handleRemoveProduct(index)}>
                <DeleteOutlined />
              </Button>
            )}
          </Row>
        </div>
      ))}

      <Button type="dashed" onClick={addProduct} block icon={<PlusOutlined />}>
        Thêm 1 sản phẩm
      </Button>
    </Card>
  );
};

export default ProductForm;