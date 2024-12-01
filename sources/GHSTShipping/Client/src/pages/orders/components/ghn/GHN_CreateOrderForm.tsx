import { Button, Card, Col, Flex, Form, Input, InputNumber, message, Radio, Row, Select, Typography, Modal } from 'antd';
import { useCallback, useEffect, useRef, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { apiCreateDeliveryOrder, apiGetPickShifts, apiUpdateDeliveryOrder } from '@/api/business.api';
import AddressComponent from '../address.component';
import NoteForm from './note-form.ghn';
import OrderInfoForm from './order-info-form.ghn';
import ProductForm, { IProduct } from './product-form.ghn';
import { setOrder, setTempOrder } from '@/features/order/orderSlice'; // Adjust the path accordingly
import { debounce } from '@/utils/common';
import { getItemWithExpiry, setItemWithExpiry } from '@/utils/common';
import { IPickShift } from '@/interface/business';
import { DeliveredProcedureOutlined } from '@ant-design/icons';
import OrderBuilder from '@/features/order/order.builder';
import { IOrder } from '@/features/order/type';
import './style.css';
import { DeliveryPricePlaneFormDto } from '@/interface/shop';
import { BasicShopInfoDto } from '@/features/shop';
import { CustomAxiosRequestConfig, request } from '@/api/base/request';
import { useParams } from 'react-router-dom';
import { shopIdSelector } from '@/stores/user.store';
import { ServiceType, ServiceTypeValue } from './ServiceType';
import AddressCache from '@/features/address';
import { ICacheAddressInfo } from '@/features/address/type';
import { AxiosRequestConfig } from 'axios';

const { Title } = Typography;
const { Option } = Select;
const { confirm } = Modal;

type Item = {
  name: string;
  weight: number;
  length: number;
  width: number;
  height: number;
  quantity: number;
  code?: string;
};

interface FormOrderGhnProps {
  isActivated: boolean;
  deliveryPricePlanes?: DeliveryPricePlaneFormDto[];
  myShops?: BasicShopInfoDto[];
}

type CalculateShippingCostInput = {
  deliveryPricePlaneId: number; // Replace with `string` if it's not a number
  weight: number;
  length: number;
  width: number;
  height: number;
};

const addressCache = new AddressCache();

const GHN_CreateOrderForm = (props: FormOrderGhnProps) => {
  const { id } = useParams<{ id: string }>();
  const editOrderData = Boolean(id) ? localStorage.getItem(`${id}`) ?? '' : '';
  const editOrder = Boolean(id) ? JSON.parse(editOrderData) : '';
  const isEdit = Boolean(editOrder) ? true : false;

  const { isActivated, deliveryPricePlanes = [], myShops = [] } = props;
  const dispatch = useDispatch();
  const session = useSelector(state => state?.user?.session);
  const shopId = useSelector(shopIdSelector);
  const deliveryPricePlaneId = useSelector(state => state?.order?.tempOrder?.deliveryPricePlaneId ?? '');

  const [pickShifts, setPickShifts] = useState<IPickShift[]>([]);
  const [form] = Form.useForm();
  const fromAddressRef = useRef<any>(null);
  const toAddressRef = useRef<any>(null);
  const [serviceTypeId, setServiceTypeId] = useState<ServiceTypeValue>(ServiceType.HangNhe);
  const [shopAddressSelected, setShopAddressSelected] = useState<BasicShopInfoDto>();
  const [allowEditSenderAddress, setAllowEditSenderAddress] = useState(true);
  const [convertedWeight, setConvertedWeight] = useState(0);
  const [isHighLight, setIsHighLight] = useState(false);
  const [allowFailedDelivery, setAllowFailedDelivery] = useState(false);
  const [editProducts, setEditProducts] = useState<IProduct[]>([
    {
      name: '',
      weight: '100',
      quantity: '1',
    },
  ]);

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

      //// DEBUG MODE
      // const _fakeOrder = fakeOrder;
      // setTimeout(() => {
      //   form.setFieldsValue(_fakeOrder);
      //   fromAddressRef.current?.update(_fakeOrder);
      //   toAddressRef.current?.update(_fakeOrder);
      // }, 500);

      // init sender address
      //// AUTO FILL SENDER ADDRESS
      // const senderAddressJson = localStorage.getItem('senderAddress') ?? '';
      // if (Boolean(senderAddressJson)) {
      //   const senderAddress = JSON.parse(senderAddressJson);
      //   if (Boolean(senderAddress)) {
      //     const senderInfo = {
      //       from_name: senderAddress['name'],
      //       from_phone: senderAddress['phone'],
      //       from_address: senderAddress['address'],
      //       from_ward_id: senderAddress['wardId'],
      //       from_ward_name: senderAddress['wardName'],
      //       from_district_id: parseInt(senderAddress['districtId']),
      //       from_district_name: senderAddress['districtName'],
      //       from_province_id: parseInt(senderAddress['provinceId']),
      //       from_province_name: senderAddress['provinceName'],
      //     };
      //     setTimeout(() => {
      //       form.setFieldsValue(senderInfo);
      //       fromAddressRef.current?.update(senderInfo);
      //     }, 300);
      //   }
      // }
    } catch (error) {
      message.error('Lỗi khi tải ca lấy hàng');
    }
  };

  const handleFormSubmit = async () => {
    try {
      const values: IOrder = await form.validateFields();
      if (values.weight > 20000) {
        console.log('Đơn hàng lớn hơn 20kg');
        // Trong đó:  2: Hàng nhẹ, 5: Hàng nặng
        values.service_type_id = ServiceType.HangNang;
      } else {
        values.service_type_id = ServiceType.HangNhe;
      }

      const orderBuilder = new OrderBuilder(values);
      const validate = orderBuilder.validateOrder();

      if (!validate.verified && values.service_type_id === ServiceType.HangNhe) {
        console.log('validate ', validate.errors);
        message.error('Thông tin đơn chưa hợp lệ, vui lòng kiểm tra lại!');
        return;
      }

      values.shopId = shopId;

      // cache to address
      handleSaveAddress(values);

      if (isEdit) {
        const response = await apiUpdateDeliveryOrder(id as string, values);
        if (response.success) {
          // cache to address
          handleSaveAddress(values);

          message.success('Cật nhật đơn hàng thành công');
          form.resetFields();
          localStorage.removeItem(`${id}}`);
          setTimeout(() => {
            location.reload();
          }, 1000);
        }
      } else {
        const response = await apiCreateDeliveryOrder(values);
        if (response.success) {
          // cache to address
          handleSaveAddress(values);

          message.success('Tạo đơn thành công');
          form.resetFields();
          setTimeout(() => {
            location.reload();
          }, 1000);
        } else {
          message.error(response.errors[0]?.description || 'Xảy ra lỗi, vui lòng kiểm tra lại');
          console.log(response.errors);
        }
      }
    } catch (error) {
      message.info('Thông tin đơn hàng chưa đủ, vui lòng kiểm tra lại!');
    }
  };

  const handleSaveAddress = (values: IOrder) => {
    const address: ICacheAddressInfo = {
      shopId: values.shopId,
      to_address: values.to_address,
      to_district_id: values.to_district_id,
      to_district_name: values.to_district_name,
      to_name: values.to_name,
      to_phone: values.to_phone,
      to_province_id: values.to_province_id,
      to_province_name: values.to_province_name,
      to_ward_id: values.to_ward_id,
      to_ward_name: values.to_ward_name,
    };
    addressCache.save(address);
  };

  const handleCalcTotalWeigh = (currentValues: any) => {
    const orderItems = currentValues['items'] as Item[];
    if (Boolean(orderItems) && orderItems?.length > 0) {
      const calculateTotalWeight = (items: Item[]): number => {
        return items.reduce((total, item) => total + (item.weight ?? 0) * (item.quantity ?? 0), 0);
      };

      const totalWeight = calculateTotalWeight(orderItems);
      form.setFieldValue('weight', totalWeight);

      // Nếu dịch vụ là hàng nặng thì tiến hàng tự động tính toán kích thước tổng đơn hàng
      if (serviceTypeId === ServiceType.HangNang) {
        const calculateTotalLength = (items: Item[]): number => {
          return items.reduce((total, item) => total + (item.length ?? 0), 0);
        };
        const calculateTotalWidth = (items: Item[]): number => {
          return items.reduce((total, item) => total + (item.width ?? 0), 0);
        };
        const calculateTotalHeight = (items: Item[]): number => {
          return items.reduce((total, item) => total + (item.height ?? 0), 0);
        };
        const totalLength = calculateTotalLength(orderItems);
        const totalWidth = calculateTotalWidth(orderItems);
        const totalHeight = calculateTotalHeight(orderItems);
        form.setFieldValue('length', totalLength);
        form.setFieldValue('width', totalWidth);
        form.setFieldValue('height', totalHeight);
      }

      if (totalWeight >= 20000 && serviceTypeId === ServiceType.HangNhe) {
        showChangeServiceTypeNotification();
        return;
      }
    }
  };

  // Debounced update to Redux
  const handleValuesChange = debounce(changedValues => {
    handleCalc(changedValues);
  }, 800);

  const handleCalc = (changedValues: any, auto_update_insurance_value: boolean = true, force_update: boolean = false) => {
    const currentValues = form.getFieldsValue();
    dispatch(setOrder({ ...currentValues, ...changedValues }));
    dispatch(setTempOrder({ ...currentValues, ...changedValues }));

    // Nếu thay đổi service type sang hàng nặng, thì set quantity về 1 cho order items
    // và disable một số fields để không cho phép nhập (tự động tính toán)
    if (Boolean(changedValues?.service_type_id)) {
      setServiceTypeId(changedValues?.service_type_id);
      const items = currentValues?.items?.map((i: any) => {
        return {
          ...i,
          quantity: 1,
        };
      });
      form.setFieldValue('items', items);
    }

    // Nếu thay đổi không phải là khối lượng đơn hàng thì tiến hành tính lại
    if (!Boolean(changedValues?.weight) || force_update) {
      handleCalcTotalWeigh(currentValues);
    }

    if (auto_update_insurance_value) {
      if (
        Boolean(changedValues?.cod_amount) &&
        Boolean(currentValues?.cod_amount) &&
        currentValues?.cod_amount >= 0 &&
        !Boolean(currentValues?.insurance_value)
      ) {
        form.setFieldValue('insurance_value', currentValues?.cod_amount);
      }
    }

    const { deliveryPricePlaneId, weight, length, width, height } = currentValues;
    if (deliveryPricePlaneId == null || weight == null || length == null || width == null || height == null) {
      console.error('All fields must be filled out');
    } else {
      calculateShippingCost({
        deliveryPricePlaneId: deliveryPricePlaneId,
        weight: Number(weight),
        length: Number(length),
        width: Number(width),
        height: Number(height),
      });
    }

    if (changedValues?.to_phone) {
      const toAddress = addressCache.get(shopId, changedValues?.to_phone);
      if (toAddress && (!Boolean(currentValues?.to_address) || !Boolean(currentValues?.to_district_name) || !Boolean(currentValues?.to_ward_name))) {
        //TODO
        setTimeout(() => {
          form.setFieldsValue(toAddress);
          toAddressRef.current?.update(toAddress);
        }, 300);
      }
    }
  };

  const showChangeServiceTypeNotification = () => {
    confirm({
      title: 'Bạn có chắc chắn muốn xóa mục này?',
      content:
        'Với đơn hàng từ 20kg trở lên, hệ thống sẽ áp dụng dịch vụ Hàng nặng. Bạn có thể chuyển dịch vụ hoặc cập nhật lại khối lượng kích thước đơn hàng.',
      okText: 'Tôi đã hiểu',
      okType: 'danger',
      cancelText: 'Cập nhật lại thông tin',
      centered: true,
      onOk() {
        setServiceTypeId(ServiceType.HangNang);
        form.setFieldValue('service_type_id', ServiceType.HangNang);
      },
      onCancel() {},
    });
  };

  const handleChangeSenderAddress = (value: string) => {
    const selectedShopAddress = myShops.find(i => i.id === value);
    if (Boolean(selectedShopAddress)) {
      setShopAddressSelected(selectedShopAddress);
    }
  };

  const fillSelectedSenderAddress = () => {
    if (Boolean(shopAddressSelected)) {
      const senderInfo = {
        from_name: shopAddressSelected?.name,
        from_phone: shopAddressSelected?.phoneNumber,
        from_address: shopAddressSelected?.address,
        from_ward_id: shopAddressSelected?.wardId,
        from_ward_name: shopAddressSelected?.wardName,
        from_district_id: parseInt(shopAddressSelected?.districtId ?? ''),
        from_district_name: shopAddressSelected?.districtName,
        from_province_id: parseInt(shopAddressSelected?.provinceId ?? ''),
        from_province_name: shopAddressSelected?.provinceName,
      };
      setTimeout(() => {
        form.setFieldsValue(senderInfo);
        fromAddressRef.current?.update(senderInfo);
      }, 300);
    }
  };

  const calculateShippingCost = async ({ deliveryPricePlaneId, weight, length, width, height }: CalculateShippingCostInput): Promise<any> => {
    const payload = {
      deliveryPricePlaneId,
      weight,
      length,
      width,
      height,
    };
    const response = await request('post', '/orders/calculate-shipping-cost', payload, {
      skipLoading: true,
    } as CustomAxiosRequestConfig);
    if (response.success) {
      const convertedWeight = response?.data?.calcOrderWeight ?? 0;
      setConvertedWeight(convertedWeight);
      setIsHighLight(convertedWeight > weight);
    }
  };

  function getCurrentShift(shifts: IPickShift[]): IPickShift | null {
    const now = new Date();
    const currentDate = formatDate(now); // Ngày hiện tại dạng DD-MM-YYYY
    const currentSeconds = now.getHours() * 3600 + now.getMinutes() * 60 + now.getSeconds();

    // Hàm chuyển ngày sang định dạng DD-MM-YYYY
    function formatDate(date: Date): string {
      const day = String(date.getDate()).padStart(2, '0');
      const month = String(date.getMonth() + 1).padStart(2, '0');
      const year = date.getFullYear();
      return `${day}-${month}-${year}`;
    }

    // Tìm ca trong ngày hiện tại
    const todayShift = shifts.find(shift => shift.title.includes(currentDate) && currentSeconds >= shift.fromTime && currentSeconds < shift.toTime);

    if (todayShift) return todayShift;

    // Nếu không tìm thấy, tìm ca của ngày hôm sau
    const nextDate = new Date(now.getTime() + 86400000); // Ngày hôm sau
    const nextDay = formatDate(nextDate); // Chuyển sang định dạng DD-MM-YYYY
    const nextDayShift = shifts.find(shift => shift.title.includes(nextDay));

    return nextDayShift || null; // Nếu không tìm thấy, trả về null
  }

  // Create a debounced version of handleCalc
  const debouncedHandleCalc = useCallback(
    debounce(formValues => {
      handleCalc(formValues, false, true);
    }, 500), // 500ms delay
    [handleCalc],
  );

  useEffect(() => {
    if (myShops && myShops.length > 0) {
      setShopAddressSelected(myShops[0]);
      if (!Boolean(myShops[0]?.address) || !Boolean(myShops[0]?.wardId)) {
        message.info('Vui lòng cập nhật địa chỉ cửa hàng');
        setAllowEditSenderAddress(false);
      }
    }
  }, [myShops]);

  useEffect(() => {
    fillSelectedSenderAddress();
  }, [shopAddressSelected]);

  useEffect(() => {
    // TODO INIT FORM DATA WITH SESSION DATA
  }, [session]);

  useEffect(() => {
    if (isEdit) {
      const editOrderData = editOrder;
      form.setFieldsValue(editOrderData);
      if (editOrderData?.cod_failed_amount > 0) {
        setAllowFailedDelivery(true);
      }
      const products: IProduct[] = editOrderData.items.map((i: any) => {
        return {
          name: i?.name,
          weight: `${i?.weight ?? 0}`,
          quantity: `${i?.quantity ?? 0}`,
        } as IProduct;
      });
      setEditProducts(products);

      setTimeout(() => {
        fromAddressRef.current?.update(editOrderData);
        toAddressRef.current?.update(editOrderData);
      }, 500);

      setTimeout(() => {
        handleCalc(editOrderData, false);
      }, 800);
    }
  }, [isEdit]);

  useEffect(() => {
    fetchPickShifts();
  }, []);

  useEffect(() => {
    if (pickShifts && pickShifts?.length > 0) {
      const currentShift = getCurrentShift(pickShifts);
      if (currentShift) {
        // auto pick shift
        form.setFieldValue('pick_shift', [currentShift?.id]);
      }
    }
  }, [pickShifts]);

  useEffect(() => {
    // Nếu là tạo đơn hàng và giá trị ghi nhớ của deliveryPricePlaneId không tồn tại
    // thì lấy giá trị đầu tiên trong danh sách
    if (!Boolean(deliveryPricePlaneId) && !isEdit && deliveryPricePlanes && deliveryPricePlanes.length > 0) {
      form.setFieldValue('deliveryPricePlaneId', deliveryPricePlanes[0]?.id);
    }
  }, [deliveryPricePlanes]);

  useEffect(() => {
    // Nếu là tạo đơn mới và có giá trị ghi nhớ thì set bảng giá theo giá trị đã ghi nhớ
    if (Boolean(deliveryPricePlaneId) && !isEdit) {
      form.setFieldValue('deliveryPricePlaneId', deliveryPricePlaneId);
    }
  }, [deliveryPricePlaneId]);

  useEffect(() => {}, []);

  const CreateOrderButton = () => {
    return (
      <Button htmlType="button" type="primary" onClick={handleFormSubmit} style={{ marginBottom: '10px', marginTop: '10px', float: 'right' }}>
        <DeliveredProcedureOutlined /> {isEdit ? 'Cập nhật' : 'Tạo đơn'}
      </Button>
    );
  };

  return (
    <div style={{ maxHeight: '78vh', overflowY: 'auto' }}>
      <Form layout="vertical" form={form} onValuesChange={handleValuesChange} disabled={!isActivated}>
        <Card style={{ marginBottom: '16px' }} className="custom-card">
          <Row gutter={[16, 16]}>
            <Col span={12}>
              <Form.Item
                label="Bảng giá"
                name="deliveryPricePlaneId"
                rules={[
                  {
                    required: true,
                    message: 'Vui lòng chọn bảng giá',
                  },
                ]}
              >
                <Select placeholder="Chọn bảng giá">
                  {deliveryPricePlanes.map(i => (
                    <Option key={i.id} value={i.id}>
                      {i.name}
                    </Option>
                  ))}
                </Select>
              </Form.Item>
            </Col>
            <Col span={12}>
              <Form.Item label="Ca lấy hàng" name="pick_shift" valuePropName="value" getValueFromEvent={value => [value]}>
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
            <Col span={24}>
              <span>Chọn địa chỉ cửa hàng</span>
              <Col span={12}>
                <Select
                  disabled={!allowEditSenderAddress}
                  onChange={handleChangeSenderAddress}
                  placeholder="Chọn địa chỉ gửi"
                  style={{ width: '100%', marginTop: '5px' }}
                  value={shopAddressSelected?.id}
                >
                  {myShops.map(i => (
                    <Option key={i.id} value={i.id}>
                      {i.name}
                    </Option>
                  ))}
                </Select>

                <Button style={{ paddingLeft: '0px' }} type="link" onClick={() => setAllowEditSenderAddress(false)}>
                  Thay đổi địa chỉ
                </Button>
                {!allowEditSenderAddress && (
                  <Button
                    style={{ paddingLeft: '0px' }}
                    danger
                    type="link"
                    onClick={() => {
                      setAllowEditSenderAddress(true);
                      fillSelectedSenderAddress();
                    }}
                  >
                    Hủy
                  </Button>
                )}
              </Col>
            </Col>
            <div className="border-top-info"></div>

            <Col span={12}>
              <Form.Item
                hidden={allowEditSenderAddress}
                label="Số điện thoại người gửi"
                name="from_phone"
                rules={[{ required: true, message: 'Vui lòng nhập số điện thoại!' }]}
              >
                <Input placeholder="Nhập số điện thoại người gửi" />
              </Form.Item>
              <Form.Item
                hidden={allowEditSenderAddress}
                label="Tên người gửi"
                name="from_name"
                rules={[{ required: true, message: 'Vui lòng nhập họ tên!' }]}
              >
                <Input placeholder="Nhập tên người gửi" />
              </Form.Item>
            </Col>
            <Col span={12}>
              <AddressComponent
                ref={fromAddressRef}
                key={'from_address'}
                form={form}
                required={true}
                hidden={allowEditSenderAddress}
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

        <Col span={24} style={{ marginLeft: '10px', marginTop: '20px' }}>
          <Flex vertical gap="middle">
            <Form.Item name="service_type_id">
              <Radio.Group defaultValue={serviceTypeId} buttonStyle="solid">
                <Radio.Button value={ServiceType.HangNhe}>{'Hàng nhẹ (< 20 kg)'}</Radio.Button>
                <Radio.Button value={ServiceType.HangNang}>{'Hàng nặng (>= 20 kg)'}</Radio.Button>
              </Radio.Group>
            </Form.Item>
          </Flex>
        </Col>

        <ProductForm
          serviceType={serviceTypeId}
          products={editProducts}
          onChange={() => {
            const formValues = form.getFieldsValue();
            debouncedHandleCalc(formValues); // Use the debounced function
          }}
          onRemove={(index: number) => {
            const formValues = form.getFieldsValue();
            if (formValues?.items && formValues?.items?.length > 0) {
              const items = formValues?.items;
              items.splice(index, 1);
              form.setFieldValue('items', items);

              const _formValues = form.getFieldsValue();
              debouncedHandleCalc(_formValues); // Use the debounced function
            }
          }}
        />
        <OrderInfoForm
          serviceType={serviceTypeId}
          allowFailedDelivery={allowFailedDelivery}
          convertedWeight={convertedWeight}
          highlight={isHighLight}
        />
        <NoteForm />

        <Card className="custom-card">
          <Col span={12} style={{ marginTop: '16px' }}>
            <Form.Item label="Hình thức thanh toán" name="payment_type_id" rules={[{ required: true, message: 'Vui lòng chọn' }]}>
              <Select placeholder="Chọn Hình thức thanh toán">
                <Option value={1}>Người gửi trả phí</Option>
                <Option value={2}>Người nhận trả phí</Option>
              </Select>
            </Form.Item>
          </Col>
        </Card>
      </Form>

      <CreateOrderButton />
    </div>
  );
};

export default GHN_CreateOrderForm;
