import { DeleteOutlined, PlusOutlined, UploadOutlined } from '@ant-design/icons';
import { Button, Card, Col, Form, Input, InputNumber, message, Row, Upload } from 'antd';
import React, { useEffect, useState } from 'react';
import { ServiceType, ServiceTypeValue } from './ServiceType';

import MyInputNumber from '../MyInputNumber';

export interface IProduct {
  name: string;
  code?: string;
  weight: string;
  length?: string;
  width?: string;
  height?: string;
  quantity: string;
}

interface ProductFormProps {
  products?: IProduct[];
  serviceType: ServiceTypeValue;
  onChange?: () => void;
  onRemove: (index: number) => void;
}

const ProductForm = (props: ProductFormProps) => {
  const { products, serviceType, onChange, onRemove } = props;
  const [_products, set_Products] = useState<IProduct[]>([{ name: '', code: '', weight: '200', quantity: '1' }]);

  const addProduct = () => {
    set_Products([..._products, { name: '', code: '', weight: '1', quantity: '1' }]);
  };

  const handleInputChange = (index: number, field: keyof IProduct, value: string) => {
    const newProducts: IProduct[] = [..._products];
    if (newProducts[index]) {
      newProducts[index][field] = value;
    }
    set_Products(newProducts);
  };

  const handleRemoveProduct = (index: number) => {
    if (_products.length === 1) {
      message.info('Đơn hàng phải có ít nhất 1 sản phẩm');
      return;
    }
    _products.splice(index, 1);
    const newProducts: IProduct[] = [..._products];
    set_Products(newProducts);
    onRemove(index);
  };

  useEffect(() => {
    if (Boolean(products)) {
      set_Products(products ?? []);
    }
  }, [products]);

  useEffect(() => {
    onChange && onChange();
  }, [_products]);

  return (
    <Card title="Thông tin sản phẩm" style={{ marginBottom: '16px', marginTop: '0' }} className="custom-card">
      {_products.map((product, index: number) => (
        <div key={index} style={{ marginBottom: 16, border: '1px solid #e8e8e8', padding: 16 }}>
          <Row gutter={[16, 16]}>
            {/* Product Name */}
            <Col span={6}>
              <Form.Item
                label={`${serviceType === ServiceType.HangNang ? 'Kiện hàng' : 'Sản phẩm'} ${index + 1}`}
                name={['items', index, 'name']}
                rules={[{ required: true, message: 'Vui lòng nhập' }]}
              >
                <Input placeholder="Nhập tên sản phẩm" value={product.name} onChange={e => handleInputChange(index, 'name', e.target.value)} />
              </Form.Item>
            </Col>

            {/* Weight */}
            <Col span={3}>
              <Form.Item
                label="KL (gram)"
                name={['items', index, 'weight']}
                rules={[
                  { required: true, message: 'Vui lòng nhập' },
                  {
                    type: 'number',
                    min: 1,
                    message: 'Giá trị phải lớn hơn 0',
                    transform: value => {
                      return Number(value) || 0;
                    },
                  },
                ]}
              >
                <MyInputNumber
                  max={50000}
                  variant="outlined"
                  placeholder="Nhập giá trị"
                  onChange={e => {
                    const value: string = e.toString();
                    handleInputChange(index, 'weight', value || '');
                  }}
                />
              </Form.Item>
            </Col>

            {serviceType === ServiceType.HangNang && (
              <Col span={3}>
                <Form.Item
                  label="Dài (cm)"
                  name={['items', index, 'length']}
                  rules={[
                    { required: true, message: 'Vui lòng nhập' },
                    {
                      type: 'number',
                      min: 1,
                      message: 'Giá trị phải lớn hơn 0',
                      transform: value => {
                        return Number(value) || 0;
                      },
                    },
                  ]}
                >
                  <MyInputNumber
                    placeholder="Nhập giá trị"
                    onChange={e => {
                      const value: string = e.toString();
                      handleInputChange(index, 'length', value || '');
                    }}
                  />
                </Form.Item>
              </Col>
            )}

            {serviceType === ServiceType.HangNang && (
              <Col span={3}>
                <Form.Item
                  label="Rộng (cm)"
                  name={['items', index, 'width']}
                  rules={[
                    { required: true, message: 'Vui lòng nhập' },
                    {
                      type: 'number',
                      min: 1,
                      message: 'Giá trị phải lớn hơn 0',
                      transform: value => {
                        return Number(value) || 0;
                      },
                    },
                  ]}
                >
                  <MyInputNumber
                    placeholder="Nhập giá trị"
                    onChange={e => {
                      const value: string = e.toString();
                      handleInputChange(index, 'width', value || '');
                    }}
                  />
                </Form.Item>
              </Col>
            )}

            {serviceType === ServiceType.HangNang && (
              <Col span={3}>
                <Form.Item
                  label="Cao (cm)"
                  name={['items', index, 'height']}
                  rules={[
                    { required: true, message: 'Vui lòng nhập' },
                    {
                      type: 'number',
                      min: 1,
                      message: 'Giá trị phải lớn hơn 0',
                      transform: value => {
                        return Number(value) || 0;
                      },
                    },
                  ]}
                >
                  <MyInputNumber
                    placeholder="Nhập giá trị"
                    onChange={e => {
                      const value: string = e.toString();
                      handleInputChange(index, 'height', value || '');
                    }}
                  />
                </Form.Item>
              </Col>
            )}

            {/* Quantity */}
            <Col span={2}>
              <Form.Item
                hidden={serviceType === ServiceType.HangNang}
                initialValue={1}
                label="Số lượng"
                name={['items', index, 'quantity']}
                rules={[
                  { required: true, message: 'Vui lòng nhập' },
                  {
                    type: 'number',
                    min: 1,
                    message: 'Số lượng phải lớn hơn 0',
                    transform: value => {
                      return Number(value) || 0;
                    },
                  },
                ]}
              >
                <MyInputNumber
                  disabled={serviceType === ServiceType.HangNang}
                  placeholder="Nhập số lượng"
                  onChange={e => {
                    const value: string = e.toString();
                    handleInputChange(index, 'quantity', value || '');
                  }}
                />
              </Form.Item>
            </Col>

            {/* Product Code */}
            <Col span={3}>
              <Form.Item hidden={serviceType === ServiceType.HangNang} label="Mã sản phẩm" name={['items', index, 'code']}>
                <Input placeholder="Nhập mã sản phẩm" onChange={e => handleInputChange(index, 'code', e.target.value)} />
              </Form.Item>
            </Col>

            {/* Image Upload */}
            <Col span={4}>
              <Form.Item label="Hình ảnh" hidden>
                <Upload>
                  <Button icon={<UploadOutlined />}>Upload</Button>
                </Upload>
              </Form.Item>
            </Col>
          </Row>

          <Row justify={'end'}>
            {_products.length > 1 && (
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
