import { Typography } from 'antd';
import { BaseType } from 'antd/es/typography/Base';

interface IPriceProps {
  value: number;
  type?: BaseType | undefined;
}

const Price = (props: IPriceProps) => {
  const { value, type = 'secondary' } = props;
  return (
    <Typography.Text type={type}>
      {new Intl.NumberFormat('vi-VN', {
        style: 'currency',
        currency: 'VND',
      }).format(value)}
    </Typography.Text>
  );
};

export default Price;
