import type { FormInstance } from 'antd';

import { Form, Input, Select } from 'antd';
import { useEffect, useState } from 'react';

import { apiGetDictricts, apiGetWards } from '@/api/business.api';

interface IReturnField {
  address: string;
  districtId: string;
  districtName: string;
  wardId: string;
  wardName: string;
  provinceId: string;
  provinceName: string;
}

interface IAddressComponentProps {
  form: FormInstance;
  returnField: IReturnField;
}

const AddressComponent = (props: IAddressComponentProps) => {
  const { form, returnField } = props;
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
      form.setFieldValue(returnField.districtName, district.districtName);
      form.setFieldValue(returnField.provinceName, district.provinceName);
      form.setFieldValue(returnField.provinceId, district.provinceID);

      form.setFieldValue(returnField.wardId, null);
      form.setFieldValue(returnField.wardName, null);
    }
  };

  const handleChangeWard = (wardCode: number) => {
    const ward = wards.find(i => i.wardCode === wardCode);
    if (ward) {
      form.setFieldValue(returnField.wardName, ward.wardName);
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
      <Form.Item hidden name={returnField.provinceId}>
        <Input />
      </Form.Item>
      <Form.Item hidden name={returnField.provinceName}>
        <Input />
      </Form.Item>

      <Form.Item label="Địa chỉ" name={returnField.address} rules={[{ required: true, message: 'Vui lòng nhập địa chỉ!' }]}>
        <Input placeholder="Nhập địa chỉ" />
      </Form.Item>

      <Form.Item label="Quận - Huyện" name={returnField.districtId}>
        <Select showSearch placeholder="Chọn quận - huyện" optionFilterProp="children" onChange={handleChangeDistrict}>
          {districts?.map(district => (
            <Select.Option key={`${district.districtID}`} value={district.districtID}>
              {district.display}
            </Select.Option>
          ))}
        </Select>
      </Form.Item>
      <Form.Item hidden name={returnField.districtName}>
        <Input />
      </Form.Item>

      <Form.Item label="Phường - Xã" name={returnField.wardId}>
        <Select showSearch placeholder="Chọn phường - xã" optionFilterProp="children" onChange={handleChangeWard}>
          {wards?.map(ward => (
            <Select.Option key={`${ward.wardCode}`} value={ward.wardCode}>
              {ward.wardName}
            </Select.Option>
          ))}
        </Select>
      </Form.Item>
      <Form.Item hidden name={returnField.wardName}>
        <Input />
      </Form.Item>
    </>
  );
};

export default AddressComponent;
