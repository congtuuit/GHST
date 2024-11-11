import { Select } from 'antd';
import { vietNameBanks } from './vietnam-banks';
import { ValueType } from 'recharts/types/component/DefaultTooltipContent';

interface VietNameBankSelectorProps {
  value?: any;
  onChange?: (value: ValueType) => void;
}
const VietNameBankSelector = (props: VietNameBankSelectorProps) => {
  const { value, onChange } = props;

  return (
    <Select
      value={value}
      onChange={onChange}
      showSearch
      style={{ width: '100%' }}
      placeholder="Chọn ngân hàng"
      optionFilterProp="label"
      filterSort={(optionA, optionB) => (optionA?.label ?? '').toLowerCase().localeCompare((optionB?.label ?? '').toLowerCase())}
      options={vietNameBanks?.map(i => {
        return {
          value: i.name,
          label: i.name + ` (${i.shortName})`,
        };
      })}
    />
  );
};

export default VietNameBankSelector;
