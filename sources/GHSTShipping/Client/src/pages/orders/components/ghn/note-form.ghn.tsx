import { InfoCircleOutlined } from '@ant-design/icons';
import { Card, Form, Input, Select } from 'antd';

const { Option } = Select;
const { TextArea } = Input;

const NoteForm = () => {
  return (
    <Card title="Lưu ý - Ghi chú" className='custom-card'>
      {/* Delivery Notes Section */}
      <Form.Item
        name="required_note"
        label="Lưu ý giao hàng"
        tooltip={{ title: 'Chọn lưu ý giao hàng', icon: <InfoCircleOutlined /> }}
        rules={[{ required: true, message: 'Vui lòng chọn' }]}
      >
        <Select placeholder="Chọn lưu ý giao hàng">
          <Option value="KHONGCHOXEMHANG">Không cho xem hàng</Option>
          <Option value="CHOXEMHANGKHONGTHU">Cho xem hàng không cho thử</Option>
          <Option value="CHOTHUHANG">Cho thử hàng</Option>
        </Select>
      </Form.Item>

      {/* Customer Order Code */}
      <Form.Item hidden label="Mã đơn khách hàng" tooltip={{ title: 'Nhập mã đơn khách hàng', icon: <InfoCircleOutlined /> }}>
        <Input placeholder="Nhập mã đơn khách hàng"/>
      </Form.Item>

      {/* Comments Section */}
      <Form.Item name="note" label="Ghi chú">
        <TextArea placeholder="Nhập ghi chú" rows={4} maxLength={250} showCount/>
      </Form.Item>
    </Card>
  );
};

export default NoteForm;
