import type { FormInstance } from 'antd';
import { Form, Input, Select } from 'antd';
import { useEffect, useState, forwardRef, useImperativeHandle } from 'react';
import { apiGetDictricts, apiGetWards } from '@/api/business.api';

interface District {
  districtID: number;
  districtName: string;
  provinceID: string;
  provinceName: string;
  display: string;
}

interface Ward {
  wardCode: number;
  wardName: string;
}

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
  required: boolean;
  hidden?: boolean;
}

const AddressComponent = forwardRef((props: IAddressComponentProps, ref) => {
  const { form, returnField, required, hidden } = props;
  const [districts, setDistricts] = useState<District[]>([]);
  const [wards, setWards] = useState<Ward[]>([]);
  const [districtId, setDistrictId] = useState<number | undefined>(undefined);
  const [isWardsLoading, setIsWardsLoading] = useState(false);

  const fetchDistricts = async () => {
    const storedDistricts = localStorage.getItem('districts');
    if (storedDistricts) {
      setDistricts(JSON.parse(storedDistricts));
    } else {
      const result = await apiGetDictricts();
      localStorage.setItem('districts', JSON.stringify(result.data));
      setDistricts(result.data);
    }
  };

  const fetchWards = async (districtId: number) => {
    setIsWardsLoading(true);
    const result = await apiGetWards(districtId);
    localStorage.setItem(`wards_${districtId}`, JSON.stringify(result.data));
    setWards(result.data);
    setIsWardsLoading(false);
  };

  const handleChangeDistrict = async (districtId: number) => {
    setDistrictId(districtId);
    const district = districts.find(d => d.districtID === districtId);
    if (district) {
      form.setFields([
        { name: returnField.districtName, value: district.districtName },
        { name: returnField.provinceName, value: district.provinceName },
        { name: returnField.provinceId, value: district.provinceID },
        { name: returnField.wardId, value: null }, // reset ward
        { name: returnField.wardName, value: null },
      ]);
    }
  };

  const handleChangeWard = (wardCode: number) => {
    const ward = wards.find(w => w.wardCode === wardCode);
    if (ward) {
      form.setFieldValue(returnField.wardName, ward.wardName);
    }
  };

  useEffect(() => {
    fetchDistricts();
  }, []);

  useEffect(() => {
    if (districtId) {
      const storedWards = localStorage.getItem(`wards_${districtId}`);
      if (storedWards) {
        setWards(JSON.parse(storedWards));
      } else {
        fetchWards(districtId);
      }
    }
  }, [districtId]);

  // Sử dụng useImperativeHandle để cho phép các component cha truy cập vào phương thức focusAddress
  useImperativeHandle(ref, () => ({
    update(values: any) {
      const distrcitId = form.getFieldValue(returnField.districtId);
      handleChangeDistrict(distrcitId).then(() => {
        form.setFields([
          { name: returnField.wardId, value: values[returnField.wardId] },
          { name: returnField.wardName, value: values[returnField.wardName] },
        ]);
      });
      
    },
  }));

  return (
    <>
      <Form.Item hidden name={returnField.provinceId}>
        <Input />
      </Form.Item>
      <Form.Item hidden name={returnField.provinceName}>
        <Input />
      </Form.Item>

      <Form.Item hidden={hidden} label="Địa chỉ" name={returnField.address} rules={[{ required, message: 'Vui lòng nhập địa chỉ!' }]}>
        <Input placeholder="Nhập địa chỉ" />
      </Form.Item>

      <Form.Item hidden={hidden} label="Quận - Huyện" name={returnField.districtId} rules={[{ required, message: 'Vui lòng chọn!' }]}>
        <Select showSearch placeholder="Chọn quận - huyện" optionFilterProp="children" onChange={handleChangeDistrict}>
          {districts.map(district => (
            <Select.Option key={district.districtID} value={district.districtID}>
              {district.display}
            </Select.Option>
          ))}
        </Select>
      </Form.Item>
      <Form.Item hidden name={returnField.districtName}>
        <Input />
      </Form.Item>

      <Form.Item hidden={hidden} label="Phường - Xã" name={returnField.wardId} rules={[{ required, message: 'Vui lòng chọn!' }]}>
        <Select showSearch placeholder="Chọn phường - xã" optionFilterProp="children" loading={isWardsLoading} onChange={handleChangeWard}>
          {wards.map(ward => (
            <Select.Option key={ward.wardCode} value={ward.wardCode}>
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
});

export default AddressComponent;
