import React from 'react';

interface NumberFormatterProps {
  value: number;
  locale?: string;
  style?: 'decimal' | 'currency' | 'percent' | 'unit';
  currency?: string;
  minimumFractionDigits?: number;
  maximumFractionDigits?: number;
  unit?: string;
}

const NumberFormatter: React.FC<NumberFormatterProps> = ({
  value,
  locale = "en-US",
  style = "decimal",
  currency,
  minimumFractionDigits,
  maximumFractionDigits,
  unit,
}) => {
  const options: Intl.NumberFormatOptions = {
    style,
    currency,
    minimumFractionDigits,
    maximumFractionDigits,
    unit,
  };

  const formattedNumber = new Intl.NumberFormat(locale, options).format(value);

  return <span>{formattedNumber}</span>;
};

export default NumberFormatter;
