import type { FC } from 'react';

import { ReactComponent as AccountSvg } from '@/assets/menu/account.svg';
import { ReactComponent as DashboardSvg } from '@/assets/menu/dashboard.svg';
import { ReactComponent as DocumentationSvg } from '@/assets/menu/documentation.svg';
import { ReactComponent as GuideSvg } from '@/assets/menu/guide.svg';
import { ReactComponent as PermissionSvg } from '@/assets/menu/permission.svg';
import { PieChartOutlined, SettingOutlined, UserOutlined, CodeSandboxOutlined } from '@ant-design/icons';

interface CustomIconProps {
  type: string;
}

const iconMap: { [key: string]: JSX.Element } = {
  guide: <GuideSvg />,
  permission: <PermissionSvg />,
  dashboard: <DashboardSvg />,
  account: <AccountSvg />,
  documentation: <DocumentationSvg />,
  report: <PieChartOutlined />,
  settings: <SettingOutlined />,
  order: <CodeSandboxOutlined />,
  customer: <UserOutlined />,
};

export const CustomIcon: FC<CustomIconProps> = ({ type }) => {
  const IconComponent = iconMap[type] || <GuideSvg />;
  return <span className="anticon">{IconComponent}</span>;
};
