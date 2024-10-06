import { apiGetDictricts, apiGetWards } from '@/api/business.api';
import { Form, FormInstance, Input, Select } from 'antd';
import { useEffect, useState } from 'react';

interface IAddressComponentProps {
  form: FormInstance;
  addressField: string;
  districtField: string;
  wardField: string;
}

const AddressComponent = (props: IAddressComponentProps) => {
  const { form, addressField, districtField, wardField } = props;

  const [districts, setDistricts] = useState<any[]>(JSON.parse(localStorage.getItem('dictricts') || '[]'));
  const [wards, setWards] = useState<any[]>(JSON.parse(localStorage.getItem('wards') || '[]'));
  const [districtId, setDistrictId] = useState<number | undefined>(undefined);

  const fetchDistricts = async () => {
    if (districts.length > 0) {
      return;
    }
    const result = await apiGetDictricts();
    localStorage.setItem('dictricts', JSON.stringify(result.data));
    setDistricts(result.data);
  };

  const fetchWards = async (districtId: number) => {
    const result = await apiGetWards(districtId);
    localStorage.setItem('wards', JSON.stringify(result.data));
    setWards(result.data);
  };

  const handleChangeDistrict = (districtId: number) => {
    setDistrictId(districtId);
    const district = districts.find(i => i.districtID === districtId);
    if (district) {
      form.setFieldValue('districtName', district.districtName);
      form.setFieldValue('provinceName', district.provinceName);
    }
  };

  const handleChangeWard = (wardCode: number) => {
    const ward = wards.find(i => i.wardCode === wardCode);
    if (ward) {
      form.setFieldValue('wardName', ward.wardName);
    }
  };

  useEffect(() => {
    fetchDistricts();
  }, []);

  useEffect(() => {
    if (Boolean(districtId)) {
      fetchWards(districtId || 0);
    }
  }, [districtId]);

  return (
    <>
      <Form.Item hidden name={'wardName'}>
        <Input />
      </Form.Item>
      <Form.Item hidden name={'districtName'}>
        <Input />
      </Form.Item>
      <Form.Item hidden name={'provinceName'}>
        <Input />
      </Form.Item>

      <Form.Item label="Địa chỉ" name={addressField} rules={[{ required: true, message: 'Vui lòng nhập địa chỉ!' }]}>
        <Input placeholder="Nhập địa chỉ" />
      </Form.Item>

      <Form.Item label="Quận - Huyện" name={districtField}>
        <Select showSearch placeholder="Chọn quận - huyện" optionFilterProp="children" onChange={handleChangeDistrict}>
          {districts?.map(district => (
            <Select.Option key={`${district.districtID}`} value={district.districtID}>
              {district.display}
            </Select.Option>
          ))}
        </Select>
      </Form.Item>

      <Form.Item label="Phường - Xã" name={wardField}>
        <Select showSearch placeholder="Chọn phường - xã" optionFilterProp="children" onChange={handleChangeWard}>
          {wards?.map(ward => (
            <Select.Option key={`${ward.wardCode}`} value={ward.wardCode}>
              {ward.wardName}
            </Select.Option>
          ))}
        </Select>
      </Form.Item>
    </>
  );
};

export default AddressComponent;
