import React from "react";
import { Avatar, Dropdown, Menu, Space, Typography } from "antd";
import { UserOutlined, LogoutOutlined } from "@ant-design/icons";

const { Text } = Typography;

const UserAvatar = (props) => {
  const { user, onLogout } = props;

  const menu = (
    <Menu>
      <Menu.Item key="profile">
        <Text>Thông tin cá nhân (comming soon)</Text>
      </Menu.Item>
      <Menu.Item key="logout" onClick={onLogout}>
        <LogoutOutlined /> Đăng xuất
      </Menu.Item>
    </Menu>
  );

  return (
    <div style={{ padding: "20px", textAlign: "right" }}>
      <Dropdown overlay={menu} placement="bottomRight">
        <Space>
          <Avatar size="large" icon={<UserOutlined />} />
          <Text>{user["name"]}</Text>
        </Space>
      </Dropdown>
    </div>
  );
};

export default UserAvatar;