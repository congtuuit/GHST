import { Button } from 'antd';
import { DeleteOutlined, EditOutlined } from '@ant-design/icons';
import { CSSProperties } from 'react';
import { ButtonType } from 'antd/es/button';

interface ActionButtonProps {
  onClick?: () => void;
  className?: string;
  iconAction?: 'delete' | 'edit';
  text?: string;
  style?: CSSProperties; // Optional style prop
  type?: ButtonType | 'danger';
  danger?: boolean;
}

const ActionButton = (props: ActionButtonProps) => {
  const { onClick, className, iconAction, text, style, type, danger } = props;
  const icon = iconAction === 'edit' ? <EditOutlined /> : iconAction === 'delete' ? <DeleteOutlined /> : '';
  return (
    <Button danger={danger} className={className} onClick={onClick} style={{ ...style }}>
      {icon} {text}
    </Button>
  );
};

export default ActionButton;
