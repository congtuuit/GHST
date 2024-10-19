import type { BaseType } from 'antd/es/typography/Base';

import { Typography } from 'antd';

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
