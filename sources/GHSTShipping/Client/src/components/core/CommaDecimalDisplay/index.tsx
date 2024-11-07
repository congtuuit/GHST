import React from 'react';
import { Typography } from 'antd';

const { Text } = Typography;

// Define the props interface
interface CommaDecimalDisplayProps {
  value: string; // String input to be formatted
  decimalPlaces?: number;
  decimalSeparator?: string;
}

const CommaDecimalDisplay: React.FC<CommaDecimalDisplayProps> = ({ value, decimalPlaces = 0, decimalSeparator = ',' }) => {
  // Convert the string to a number
  const parsedNumber = parseFloat(value);

  // Check if the conversion was successful
  if (isNaN(parsedNumber)) {
    return <Text>{value}</Text>; // Handle invalid input
  }

  const formattedNumber = parsedNumber
    .toFixed(decimalPlaces) // Ensure two decimal places
    .replace(/\B(?=(\d{3})+(?!\d))/g, decimalSeparator) // Add space for thousands
    .replace('.', ','); // Replace dot with comma for decimals

  return <Text>{formattedNumber}</Text>;
};

export default CommaDecimalDisplay;
