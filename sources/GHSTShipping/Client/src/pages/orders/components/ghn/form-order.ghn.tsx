import type { ISenderAddress } from './sender-address.form';
import type { ICreateDeliveryOrderRequest } from '@/interface/business';
import type { LoginResult } from '@/interface/user/login';
import { Button, Card, Col, Form, Input, message, Row, Select, Typography } from 'antd';
import { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import { apiCreateDeliveryOrder, apiGetPickShifts } from '@/api/business.api';
import AddressComponent from '../address.component';
import NoteForm from './note-form.ghn';
import OrderInfoForm from './order-info-form.ghn';
import ProductForm from './product-form.ghn';
import SenderAddressForm from './sender-address.form';
import './style.css';

const { Title } = Typography;
const { Option } = Select;

interface IPickShift {
  id: number;
  title: string;
  fromTime: number;
  toTime: number;
}

const FormOrderGhn = () => {
  const session = useSelector(state => state?.user?.session as LoginResult);
  const [pickShifts, setPickShifts] = useState<IPickShift[]>([]);
  const [openSenderAddressForm, setOpenSenderAddressForm] = useState(false);
  const [senderAddress, setSenderAddress] = useState<ISenderAddress>(JSON.parse(localStorage.getItem('senderAddress') ?? '{}') as ISenderAddress);
  const [form] = Form.useForm();

  const handleChangeSenderAddress = (values: ISenderAddress) => {
    localStorage.setItem('senderAddress', JSON.stringify(values));
    setSenderAddress(values);
    setOpenSenderAddressForm(false);
  };

  const fetchPickShifts = async () => {
    const result = await apiGetPickShifts();

    setPickShifts(result.data);
  };

  const handleSubmit = () => {
    form.validateFields().then(async values => {
      const payload = {
        ...values,
        to_district_id: values.to_district_id.toString(),
        to_ward_code: values.to_ward_code.toString(),
        from_ward_name: values.wardName,
        from_district_name: values.districtName,
        from_province_name: values.provinceName,
        pick_shift: [values.pick_shift],
      };

      localStorage.setItem(
        'senderAddress',
        JSON.stringify({
          from_name: payload.from_name,
          from_phone: payload.from_phone,
          from_address: payload.from_address,
          from_ward_name: payload.from_ward_name,
          from_district_name: payload.from_district_name,
          from_province_name: payload.from_province_name,
        }),
      );

      const response = await apiCreateDeliveryOrder(payload as ICreateDeliveryOrderRequest);
      if (response.success) {
        message.success('Tạo đơn thành công');
      } else {
        message.error(response.errors[0].description);
      }
    });
  };

  useEffect(() => {
    form.setFieldsValue({
      from_name: senderAddress?.name,
      from_phone: senderAddress?.phone,
      from_address: senderAddress?.address,
      from_ward_name: senderAddress?.wardName,
      from_district_name: senderAddress?.districtName,
      from_province_name: senderAddress?.provinceName,
    });
  }, [senderAddress]);

  useEffect(() => {
    let mySenderAddress: ISenderAddress = senderAddress ?? {};

    if (session && session?.phoneNumber) {
      form.setFieldValue('from_phone', session?.phoneNumber);
      mySenderAddress = {
        ...mySenderAddress,
        phone: session?.phoneNumber,
      };
    }

    if (session && session?.fullName) {
      form.setFieldValue('from_name', session?.fullName);
      mySenderAddress = {
        ...mySenderAddress,
        name: session?.fullName,
      };
    }

    setSenderAddress({ ...mySenderAddress });
  }, [session, form]);

  useEffect(() => {
    fetchPickShifts();
    // form.setFieldsValue({
    //   ...createOrderFakeData,
    // });
  }, []);

  return (
    <div>
      <Button htmlType="button" type="primary" onClick={handleSubmit} style={{ marginBottom: '10px', marginTop: '10px' }}>
        Hoàn tất
      </Button>
      <Form layout="vertical" form={form}>
        <Card style={{ marginBottom: '16px' }}>
          <Row gutter={[16, 16]}>
            <Col span={24}>
              <Title style={{ marginTop: '0px' }} level={4}>
                Bên gửi
              </Title>
            </Col>
            <div className="border-top-info"></div>
            <Col span={6}>
              <Form.Item label="Số điện thoại người gửi" name="from_phone" rules={[{ required: true, message: 'Vui lòng nhập số điện thoại!' }]}>
                <Input placeholder="Nhập số điện thoại người gửi" />
              </Form.Item>

              <Form.Item label="Tên người gửi" name="from_name" rules={[{ required: true, message: 'Vui lòng nhập họ tên!' }]}>
                <Input placeholder="Nhập tên người gửi" />
              </Form.Item>
            </Col>

            <Col span={6}>
              <AddressComponent form={form} addressField="from_address" districtField="district" wardField="ward" />
            </Col>

            <Col span={12}>
              <Form.Item label="Ca lấy hàng" name="pick_shift">
                <Select placeholder="Chọn ca lấy hàng">
                  {pickShifts.map(i => (
                    <Option value={i.id}>{i.title}</Option>
                  ))}
                </Select>
              </Form.Item>

              <Form.Item hidden name="return_phone"></Form.Item>
              <Form.Item hidden name="return_address"></Form.Item>
              <Form.Item hidden name="return_district_id"></Form.Item>
              <Form.Item hidden name="return_ward_code"></Form.Item>

              {/* confirm */}
              <Form.Item hidden name="pick_station_id"></Form.Item>
              <Form.Item hidden name="deliver_station_id"></Form.Item>
              <Form.Item hidden name="service_id"></Form.Item>
              <Form.Item hidden name="service_type_id"></Form.Item>
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
              <AddressComponent form={form} addressField="to_address" districtField="to_district_id" wardField="to_ward_code" />
            </Col>
          </Row>
        </Card>

        <ProductForm />

        <OrderInfoForm />

        {/* Check note */}
        <NoteForm />

        <Col span={12} style={{ marginTop: '16px' }}>
          <Form.Item label="Hình thức thanh toán" name="payment_type_id">
            <Select placeholder="Chọn Hình thức thanh toán">
              <Option value="1">Người gửi trả phí</Option>
              <Option value="2">Người nhận trả phí</Option>
            </Select>
          </Form.Item>
        </Col>
      </Form>

      <SenderAddressForm open={openSenderAddressForm} onCancel={() => setOpenSenderAddressForm(false)} onSubmit={handleChangeSenderAddress} />
    </div>
  );
};

export default FormOrderGhn;
