import React from 'react';
import { Modal, Button } from 'antd';

interface CommonDialogProps {
  title: string;
  content: React.ReactNode;
  isVisible: boolean;
  onConfirm: () => void;
  onCancel: () => void;
  confirmText?: string;
  cancelText?: string;
  confirmLoading?: boolean;
}

const CommonDialog: React.FC<CommonDialogProps> = ({
  title,
  content,
  isVisible,
  onConfirm,
  onCancel,
  confirmText = 'Confirm',
  cancelText = 'Cancel',
  confirmLoading = false,
}) => {
  return (
    <Modal
      title={title}
      visible={isVisible}
      onOk={onConfirm}
      onCancel={onCancel}
      confirmLoading={confirmLoading}
      footer={[
        <Button key="cancel" onClick={onCancel}>
          {cancelText}
        </Button>,
        <Button
          key="confirm"
          type="primary"
          onClick={onConfirm}
          loading={confirmLoading}
        >
          {confirmText}
        </Button>,
      ]}
    >
      {content}
    </Modal>
  );
};

export default CommonDialog;
