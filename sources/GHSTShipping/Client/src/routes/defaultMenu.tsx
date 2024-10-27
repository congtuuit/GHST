import type { MenuList } from '@/interface/layout/menu.interface';

const defaultMenu: MenuList = [
  {
    code: 'dashboard',
    label: {
      zh_CN: '首页',
      en_US: 'Dashboard',
      vi_VN: 'Bảng điều khiển', // Vietnamese translation
    },
    icon: 'dashboard',
    path: '/dashboard',
  },
  // {
  //   code: 'operation-report',
  //   label: {
  //     zh_CN: '',
  //     en_US: 'Operation report',
  //     vi_VN: 'Báo cáo vận hành', // Vietnamese translation
  //   },
  //   icon: 'report',
  //   path: '/operation-report',
  // },
  {
    code: 'order',
    label: {
      zh_CN: '',
      en_US: 'Order',
      vi_VN: 'Quản lý đơn hàng', // Vietnamese translation
    },
    icon: 'order',
    path: '/order',
    children: [
      {
        code: 'order_list',
        label: {
          zh_CN: '',
          en_US: 'Order',
          vi_VN: 'DS đơn hàng', // Vietnamese translation
        },
        icon: 'order',
        path: '/order/list',
      },
      {
        code: 'order_create',
        label: {
          zh_CN: '',
          en_US: 'Create order',
          vi_VN: 'Tạo đơn hàng', // Vietnamese translation
        },
        icon: 'order',
        path: '/order/create',
      },
    ],
  },
  // {
  //   code: 'documentation',
  //   label: {
  //     zh_CN: '文档',
  //     en_US: 'Documentation',
  //     vi_VN: 'Tài liệu', // Vietnamese translation
  //   },
  //   icon: 'documentation',
  //   path: '/documentation',
  // },
  // {
  //   code: 'guide',
  //   label: {
  //     zh_CN: '引导',
  //     en_US: 'Guide',
  //     vi_VN: 'Hướng dẫn', // Vietnamese translation
  //   },
  //   icon: 'guide',
  //   path: '/guide',
  // },
];

export default defaultMenu;
