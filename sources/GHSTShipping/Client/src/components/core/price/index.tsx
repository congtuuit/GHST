import type { BaseType } from 'antd/es/typography/Base';
import { CSSProperties } from 'react';

import { Typography } from 'antd';

interface IPriceProps {
  value: number;
  type?: BaseType | undefined;
  style?: CSSProperties; // Optional style prop
}

const Price = (props: IPriceProps) => {
  const { value, type = 'secondary', style } = props;

  return (
    <Typography.Text type={type} style={{ ...style }}>
      {new Intl.NumberFormat('vi-VN', {
        style: 'currency',
        currency: 'VND',
      }).format(value)}
    </Typography.Text>
  );
};

export default Price;
