import type { FC } from 'react';
import type { PathRouteProps } from 'react-router';

import { Button, Result } from 'antd';
import { useSelector } from 'react-redux';
import { useLocation } from 'react-router';
import { Navigate, useNavigate } from 'react-router-dom';

import { useLocale } from '@/locales';

const PrivateRoute: FC<PathRouteProps> = props => {
  const { logged, session } = useSelector(state => state.user);
  const navigate = useNavigate();
  const { formatMessage } = useLocale();
  const location = useLocation();

  if (!logged && !Boolean(session)) {
    return <Navigate to="/login" />;
  }

  return logged ? (
    (props.element as React.ReactElement)
  ) : (
    <Result
      status="403"
      title="403"
      subTitle={formatMessage({ id: 'gloabal.tips.unauthorized' })}
      extra={
        <Button type="primary" onClick={() => navigate(`/login${'?from=' + encodeURIComponent(location.pathname)}`, { replace: true })}>
          {formatMessage({ id: 'gloabal.tips.goToLogin' })}
        </Button>
      }
    />
  );
};

export default PrivateRoute;
