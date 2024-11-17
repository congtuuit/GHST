import React from 'react';
import { Button, message } from 'antd';
import { CopyOutlined } from '@ant-design/icons';

interface CopyTextButtonProps {
  text: string;
}

const CopyTextButton: React.FC<CopyTextButtonProps> = ({ text }) => {
  const handleCopy = async () => {
    try {
      await navigator.clipboard.writeText(text);
      message.success('Đã sao chép!');
    } catch (error) {
      console.error('Sao chép không thành công:', error);
      message.error('Không sao chép được!');
    }
  };

  return <Button type="text" icon={<CopyOutlined />} onClick={handleCopy}></Button>;
};

export default CopyTextButton;
