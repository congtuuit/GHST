import { Button, Card, Checkbox, Col, Form, Input, Radio, RadioChangeEvent, Row, Select, Table } from 'antd';
import { type FC, useEffect, useState } from 'react';
import { useDispatch } from 'react-redux';
import { useLocation, useNavigate } from 'react-router-dom';

import { supplierKeys, suppliers } from '@/constants/data';
import { LocaleFormatter, useLocale } from '@/locales';
import { formatSearch } from '@/utils/formatSearch';

import FormOrderGhn from './components/ghn/form-order.ghn';
import { IOrderStatus, orderStatuses } from './orderStatus';

const { Option } = Select;

const CreateOrderPage = () => {
  const [selectedSupplier, setSelectedSupplier] = useState<string>(supplierKeys.GHN);

  const handleChange = (value: string) => {
    setSelectedSupplier(value);
  };

  return (
    <Card>
      <span>Đơn vị vận chuyển</span>
      <Col span={12}>
        <Select
          value={selectedSupplier}
          onChange={handleChange}
          style={{ width: '100%', maxWidth: '300px' }} // Adjust width as needed
          placeholder="Chọn đơn vị vận chuyển"
        >
          {suppliers.map(i => (
            <Option key={i} value={i}>
              {i}
            </Option>
          ))}
        </Select>
      </Col>
      <hr style={{ marginTop: '20px', borderTop: '1px dashed rgb(217 217 217)' }} />
      <Col span={24}>{selectedSupplier === supplierKeys.GHN && <FormOrderGhn />}</Col>
    </Card>
  );
};

export default CreateOrderPage;
