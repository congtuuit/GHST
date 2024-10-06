import React, { useEffect, useState } from 'react';
import { Input, message } from 'antd';

interface MyInputNumberProps {
  allowDecimal?: boolean; // Option to allow decimal numbers
  placeholder?: string; // Custom placeholder text
  value?: string; // Controlled value
  defaultValue?: string; // Default value (uncontrolled input)
  max?: number;
  onChange?: (value: number) => void; // Callback to handle changes in the input
}

const MyInputNumber: React.FC<MyInputNumberProps> = ({
  allowDecimal = false,
  placeholder = 'Enter numbers only',
  value,
  defaultValue = '', // Default to empty string if not provided
  max = 20000,
  onChange,
}) => {
  const [inputValue, setInputValue] = useState<string>(defaultValue);

  useEffect(() => {
    if (value !== undefined) {
      setInputValue(formatNumberWithCommas(value));
    }
  }, [value]);

  useEffect(() => {
    if (inputValue) {
      const $value: number = parseInt(inputValue, 10);
      if ($value > 20000) {
        message.error('Đơn hàng không vượt quá 20kg');
      }
    }
  }, [inputValue]);

  // Function to format number with commas
  const formatNumberWithCommas = (value: string) => {
    const [integerPart, decimalPart] = value.split('.'); // Split on decimal point
    const formattedInteger = integerPart.replace(/\B(?=(\d{3})+(?!\d))/g, ',');
    return decimalPart ? `${formattedInteger}.${decimalPart}` : formattedInteger;
  };

  // Function to remove commas from the formatted number
  const removeCommas = (value: string) => {
    return value.replace(/,/g, '');
  };

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const newValue = e.target.value;

    // Remove commas for numeric comparison and validation
    const numericValueWithoutCommas = removeCommas(newValue);

    // Regex to allow only numbers or decimals
    const regex = allowDecimal ? /^[0-9]*\.?[0-9]*$/ : /^[0-9]*$/;

    if (regex.test(numericValueWithoutCommas) || newValue === '') {
      const numericValue = allowDecimal
        ? parseFloat(numericValueWithoutCommas)
        : parseInt(numericValueWithoutCommas, 10);

      // Check if the number exceeds the max value
      if (!max || numericValue <= max || newValue === '') {
        const formattedValue = formatNumberWithCommas(numericValueWithoutCommas);
        setInputValue(formattedValue);

        // Call onChange with the raw number (without commas)
        if (onChange) {
          onChange(Number(numericValueWithoutCommas));
        }
      }
    }
  };

  return (
    <Input
      placeholder={placeholder}
      value={inputValue} // Display current state
      onChange={handleInputChange}
    />
  );
};

export default MyInputNumber;
