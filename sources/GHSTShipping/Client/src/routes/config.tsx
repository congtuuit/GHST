import type { FC, ReactElement } from 'react';
import type { PathRouteProps } from 'react-router';
import { useIntl } from 'react-intl';
import PrivateRoute from './privateRoute';
import { Role } from '@/interface/user/login';
import { useSelector } from 'react-redux';
import { rolesSelector } from '@/stores/user.store';
import { Navigate } from 'react-router-dom';

export interface WrapperRouteProps extends PathRouteProps {
  /** document title locale id */
  titleId: string;
  /** authorization？ */
  auth?: boolean;
  element: React.ReactNode | null;
  allowedRoles?: Role[];
}

const WrapperRouteComponent: FC<WrapperRouteProps> = ({ titleId, auth = true, allowedRoles, ...props }) => {
  const roles = useSelector(rolesSelector);
  const hasPermission = !Boolean(allowedRoles) || roles.some(role => allowedRoles?.includes(role));
  const { formatMessage } = useIntl();
  if (titleId) {
    document.title = formatMessage({
      id: titleId,
    });
  }

  if (auth) {
    if (hasPermission) {
      return <PrivateRoute {...props} />;
    } else {
      return <Navigate to="/unauthorized" />; // Chuyển hướng đến trang không được phép
    }
  } else {
    return props.element as ReactElement;
  }
};

export default WrapperRouteComponent;
