import { Button, Card, Col, Form, Input, message, Row, Select, Typography } from 'antd';
import { useEffect, useRef, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { apiCreateDeliveryOrder, apiGetPickShifts } from '@/api/business.api';
import AddressComponent from '../address.component';
import NoteForm from './note-form.ghn';
import OrderInfoForm from './order-info-form.ghn';
import ProductForm from './product-form.ghn';
import { setOrder } from '@/features/order/orderSlice'; // Adjust the path accordingly
import { debounce } from '@/utils/common';
import { getItemWithExpiry, setItemWithExpiry } from '@/utils/common';
import { IPickShift } from '@/interface/business';
import './style.css';
import { fakeOrder } from './create-fake-data';
import { DeliveredProcedureOutlined } from '@ant-design/icons';
import OrderBuilder from '@/features/order/order.builder';

const { Title } = Typography;
const { Option } = Select;

const FormOrderGhn = () => {
  const dispatch = useDispatch();
  const session = useSelector(state => state?.user?.session);
  const [pickShifts, setPickShifts] = useState<IPickShift[]>([]);
  const [form] = Form.useForm();
  const fromAddressRef = useRef<any>(null);
  const toAddressRef = useRef<any>(null);

  const fetchPickShifts = async () => {
    try {
      let result = getItemWithExpiry('pickShifts');
      if (Boolean(result)) {
        setPickShifts(result.data);
      } else {
        result = await apiGetPickShifts();
        setPickShifts(result.data);

        // cache pick shift 10 mins
        setItemWithExpiry('pickShifts', result, 10 * 60 * 1000);
      }

      const _fakeOrder = fakeOrder;
      setTimeout(() => {
        form.setFieldsValue(_fakeOrder);
        fromAddressRef.current?.update(_fakeOrder);
        toAddressRef.current?.update(_fakeOrder);
      }, 500);
    } catch (error) {
      message.error('Lỗi khi tải ca lấy hàng');
    }
  };

  const handleFormSubmit = async () => {
    try {
      const values = await form.validateFields();

      const orderBuilder = new OrderBuilder(values);
      const validate = orderBuilder.validateOrder();

      // service_type_id tự tính để 
      // Trong đó:  2: Hàng nhẹ, 5: Hàng nặng

      console.log('validate ', validate);
      console.log('values ', values);

      return;

      // Dispatch to Redux
      dispatch(setOrder(values));

      const response = await apiCreateDeliveryOrder(values);
      if (response.success) {
        message.success('Tạo đơn thành công');
      } else {
        message.error(response.errors[0]?.description || 'Đã xảy ra lỗi');
      }
    } catch (error) {
      message.info("Thông tin đơn hàng chưa đủ, vui lòng kiểm tra lại!");
    }
  };

  // Debounced update to Redux
  const handleValuesChange = debounce(changedValues => {
    const currentValues = form.getFieldsValue();
    console.log(currentValues);

    return;
    dispatch(setOrder({ ...currentValues, ...changedValues }));
  }, 300);

  useEffect(() => {
    // TODO INIT FORM DATA WITH SESSION DATA
  }, [session]);

  useEffect(() => {
    fetchPickShifts();
  }, []);

  const CreateOrderButton = () => {
    return (
      <Button htmlType="button" type="primary" onClick={handleFormSubmit} style={{ marginBottom: '10px', marginTop: '10px', float: 'right' }}>
        <DeliveredProcedureOutlined /> Tạo đơn
      </Button>
    );
  };

  return (
    <div>
      <Form layout="vertical" form={form} onValuesChange={handleValuesChange}>
        <Card style={{ marginBottom: '16px' }}>
          <Row>
            <Col span={12}>
              <Form.Item label="Ca lấy hàng" name="pick_shift">
                <Select placeholder="Chọn ca lấy hàng">
                  {pickShifts.map(i => (
                    <Option key={i.id} value={i.id}>
                      {i.title}
                    </Option>
                  ))}
                </Select>
              </Form.Item>
            </Col>
          </Row>
          <Row gutter={[16, 16]}>
            <Col span={24}>
              <Title style={{ marginTop: '0px' }} level={4}>
                Bên gửi
              </Title>
            </Col>
            <div className="border-top-info"></div>
            <Col span={12}>
              <Form.Item label="Số điện thoại người gửi" name="from_phone" rules={[{ required: true, message: 'Vui lòng nhập số điện thoại!' }]}>
                <Input placeholder="Nhập số điện thoại người gửi" />
              </Form.Item>
              <Form.Item label="Tên người gửi" name="from_name" rules={[{ required: true, message: 'Vui lòng nhập họ tên!' }]}>
                <Input placeholder="Nhập tên người gửi" />
              </Form.Item>
            </Col>
            <Col span={12}>
              <AddressComponent
                ref={fromAddressRef}
                key={'from_address'}
                form={form}
                required={true}
                returnField={{
                  address: 'from_address',
                  districtId: 'from_district_id',
                  districtName: 'from_district_name',
                  wardId: 'from_ward_id',
                  wardName: 'from_ward_name',
                  provinceId: 'from_province_id',
                  provinceName: 'from_province_name',
                }}
              />
            </Col>
          </Row>
          <hr style={{ marginTop: '20px', borderTop: '1px dashed rgb(217 217 217)' }} />
          <Row gutter={[16, 0]}>
            <Col span={24}>
              <Title style={{ marginTop: '0px' }} level={4}>
                Bên nhận
              </Title>
            </Col>
            <Col span={12}>
              <Form.Item label="Số điện thoại" name="to_phone" rules={[{ required: true, message: 'Vui lòng nhập số điện thoại!' }]}>
                <Input placeholder="Nhập số điện thoại" />
              </Form.Item>
              <Form.Item label="Họ tên" name="to_name" rules={[{ required: true, message: 'Vui lòng nhập họ tên!' }]}>
                <Input placeholder="Nhập họ tên" />
              </Form.Item>
            </Col>
            <Col span={12}>
              <AddressComponent
                ref={toAddressRef}
                key={'to_address'}
                required={true}
                form={form}
                returnField={{
                  address: 'to_address',
                  districtId: 'to_district_id',
                  districtName: 'to_district_name',
                  wardId: 'to_ward_id',
                  wardName: 'to_ward_name',
                  provinceId: 'to_province_id',
                  provinceName: 'to_province_name',
                }}
              />
            </Col>
          </Row>
        </Card>

        <ProductForm />
        <OrderInfoForm />
        <NoteForm />

        <Col span={12} style={{ marginTop: '16px' }}>
          <Form.Item label="Hình thức thanh toán" name="payment_type_id" rules={[{ required: true, message: 'Vui lòng chọn' }]}>
            <Select placeholder="Chọn Hình thức thanh toán">
              <Option value={1}>Người gửi trả phí</Option>
              <Option value={2}>Người nhận trả phí</Option>
            </Select>
          </Form.Item>
        </Col>
      </Form>
      <CreateOrderButton />
    </div>
  );
};

export default FormOrderGhn;
