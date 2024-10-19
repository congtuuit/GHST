import type { FC } from 'react';
import type { RouteObject } from 'react-router';

import { lazy } from 'react';
import { Navigate } from 'react-router';
import { useRoutes } from 'react-router-dom';

import CustomerPage from '@/pages/customer';
import CustomerPricePage from '@/pages/customer-price';
import Dashboard from '@/pages/dashboard';
import DeliveryConfigPage from '@/pages/delivery-config';
import ForgotPasswordPage from '@/pages/forgot-password';
import LayoutPage from '@/pages/layout';
import LoginPage from '@/pages/login';
import OrdersPage from '@/pages/orders';
import CreateOrderPage from '@/pages/orders/create';
import RegisterPage from '@/pages/register';
import ResetPasswordPage from '@/pages/reset-password';

import WrapperRouteComponent from './config';

const NotFound = lazy(() => import(/* webpackChunkName: "404'"*/ '@/pages/404'));
const Documentation = lazy(() => import(/* webpackChunkName: "404'"*/ '@/pages/doucumentation'));
const Guide = lazy(() => import(/* webpackChunkName: "guide'"*/ '@/pages/guide'));
const RoutePermission = lazy(() => import(/* webpackChunkName: "route-permission"*/ '@/pages/permission/route'));
const FormPage = lazy(() => import(/* webpackChunkName: "form'"*/ '@/pages/components/form'));
const TablePage = lazy(() => import(/* webpackChunkName: "table'"*/ '@/pages/components/table'));
const SearchPage = lazy(() => import(/* webpackChunkName: "search'"*/ '@/pages/components/search'));
const TabsPage = lazy(() => import(/* webpackChunkName: "tabs'"*/ '@/pages/components/tabs'));
const AsidePage = lazy(() => import(/* webpackChunkName: "aside'"*/ '@/pages/components/aside'));
const RadioCardsPage = lazy(() => import(/* webpackChunkName: "radio-cards'"*/ '@/pages/components/radio-cards'));

// const BusinessBasicPage = lazy(() => import(/* webpackChunkName: "basic-page" */ '@/pages/business/basic'));
// const BusinessWithSearchPage = lazy(() => import(/* webpackChunkName: "with-search" */ '@/pages/business/with-search'));
// const BusinessWithAsidePage = lazy(() => import(/* webpackChunkName: "with-aside" */ '@/pages/business/with-aside'));
// const BusinessWithRadioCardsPage = lazy(
//   () => import(/* webpackChunkName: "with-aside" */ '@/pages/business/with-radio-cards'),
// );
// const BusinessWithTabsPage = lazy(() => import(/* webpackChunkName: "with-tabs" */ '@/pages/business/with-tabs'));

const routeList: RouteObject[] = [
  {
    path: '/login',
    element: <WrapperRouteComponent element={<LoginPage />} titleId="title.login" auth={false} />,
  },
  {
    path: '/register',
    element: <WrapperRouteComponent element={<RegisterPage />} titleId="title.register" auth={false} />,
  },
  {
    path: '/forgot-password',
    element: <WrapperRouteComponent element={<ForgotPasswordPage />} titleId="title.forgotPassword" auth={false} />,
  },
  {
    path: '/reset-password',
    element: <WrapperRouteComponent element={<ResetPasswordPage />} titleId="title.resetPassword" auth={false} />,
  },
  {
    path: '/',
    element: <WrapperRouteComponent element={<LayoutPage />} titleId="" />,
    children: [
      {
        path: '',
        element: <Navigate to="dashboard" />,
      },
      {
        path: 'dashboard',
        element: <WrapperRouteComponent element={<Dashboard />} titleId="title.dashboard" />,
      },
      {
        path: 'customer/list',
        element: <WrapperRouteComponent element={<CustomerPage />} titleId="title.customer" />,
      },
      {
        path: 'customer/price-plan',
        element: <WrapperRouteComponent element={<CustomerPricePage />} titleId="title.customerPrice" />,
      },
      {
        path: 'order/list',
        element: <WrapperRouteComponent element={<OrdersPage />} titleId="title.orderList" />,
      },
      {
        path: 'order/create',
        element: <WrapperRouteComponent element={<CreateOrderPage />} titleId="title.createOrder" />,
      },
      {
        path: 'settings',
        element: <WrapperRouteComponent element={<DeliveryConfigPage />} titleId="title.deliveryConfig" />,
      },

      {
        path: 'documentation',
        element: <WrapperRouteComponent element={<Documentation />} titleId="title.documentation" />,
      },
      {
        path: 'guide',
        element: <WrapperRouteComponent element={<Guide />} titleId="title.guide" />,
      },
      {
        path: 'permission/route',
        element: <WrapperRouteComponent element={<RoutePermission />} titleId="title.permission.route" auth />,
      },
      {
        path: 'component/form',
        element: <WrapperRouteComponent element={<FormPage />} titleId="title.account" />,
      },
      {
        path: 'component/table',
        element: <WrapperRouteComponent element={<TablePage />} titleId="title.account" />,
      },
      {
        path: 'component/search',
        element: <WrapperRouteComponent element={<SearchPage />} titleId="title.account" />,
      },
      {
        path: 'component/tabs',
        element: <WrapperRouteComponent element={<TabsPage />} titleId="title.account" />,
      },
      {
        path: 'component/aside',
        element: <WrapperRouteComponent element={<AsidePage />} titleId="title.account" />,
      },
      {
        path: 'component/radio-cards',
        element: <WrapperRouteComponent element={<RadioCardsPage />} titleId="title.account" />,
      },
      {
        path: '*',
        element: <WrapperRouteComponent element={<NotFound />} titleId="title.notFount" auth={false} />,
      },
    ],
  },
];

const RenderRouter: FC = () => {
  const element = useRoutes(routeList);

  return element;
};

export default RenderRouter;
