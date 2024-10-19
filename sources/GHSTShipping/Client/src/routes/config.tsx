import type { FC, ReactElement } from 'react';
import type { PathRouteProps } from 'react-router';
import { useIntl } from 'react-intl';
import PrivateRoute from './privateRoute';

export interface WrapperRouteProps extends PathRouteProps {
  /** document title locale id */
  titleId: string;
  /** authorization？ */
  auth?: boolean;
  element: React.ReactNode | null;
}

const WrapperRouteComponent: FC<WrapperRouteProps> = ({ titleId, auth = true, ...props }) => {
  const { formatMessage } = useIntl();
  if (titleId) {
    document.title = formatMessage({
      id: titleId,
    });
  }

  return auth ? <PrivateRoute {...props} /> : (props.element as ReactElement);
};

export default WrapperRouteComponent;
