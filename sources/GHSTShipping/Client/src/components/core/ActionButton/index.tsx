import { Button } from 'antd';
import { DeleteOutlined, EditOutlined } from '@ant-design/icons';

interface ActionButtonProps {
  onClick?: () => void;
  className?: string;
  iconAction?: "delete" | "edit";
  text?: string;
}

const ActionButton = (props: ActionButtonProps) => {
  const { onClick, className, iconAction, text } = props;
  const icon = iconAction === "edit" ? <EditOutlined /> : iconAction === "delete" ? <DeleteOutlined /> : ""
  return (
    <Button className={className} size="small" onClick={onClick}>
      {icon} {text}
    </Button>
  );
};

export default ActionButton;
