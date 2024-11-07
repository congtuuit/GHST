import { Button } from 'antd';
import { RollbackOutlined } from '@ant-design/icons';
import { useNavigate } from 'react-router-dom';
import React, { CSSProperties } from 'react';

interface GoBackButtonProps {
  style?: CSSProperties; // Optional style prop
}

const GoBackButton: React.FC<GoBackButtonProps> = ({ style }) => {
  const navigate = useNavigate();

  const handleGoBack = () => {
    navigate(-1); // This navigates back to the previous page
  };

  return (
    <Button type="link" style={{ ...style }} onClick={handleGoBack}>
      <RollbackOutlined /> Quay lại
    </Button>
  );
};

export default GoBackButton;
