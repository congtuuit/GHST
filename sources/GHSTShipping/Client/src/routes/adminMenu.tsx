import { MenuList } from '@/interface/layout/menu.interface';

const adminMenu: MenuList = [
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
  {
    code: 'customer',
    label: {
      zh_CN: '',
      en_US: 'Customer',
      vi_VN: 'Khách hàng', // Vietnamese translation
    },
    icon: 'customer',
    path: '/customer',
  },
  {
    code: 'settings',
    label: {
      zh_CN: '',
      en_US: 'Settings',
      vi_VN: 'Cấu hình hệ thống', // Vietnamese translation
    },
    icon: 'settings',
    path: '/settings',
  },
  {
    code: 'operation-report',
    label: {
      zh_CN: '',
      en_US: 'Operation report',
      vi_VN: 'Báo cáo vận hành', // Vietnamese translation
    },
    icon: 'report',
    path: '/operation-report',
  },
  {
    code: 'order',
    label: {
      zh_CN: '',
      en_US: 'Order',
      vi_VN: 'Đơn hàng', // Vietnamese translation
    },
    icon: 'order',
    path: '/order',
  },
  {
    code: 'documentation',
    label: {
      zh_CN: '文档',
      en_US: 'Documentation',
      vi_VN: 'Tài liệu', // Vietnamese translation
    },
    icon: 'documentation',
    path: '/documentation',
  },
  {
    code: 'guide',
    label: {
      zh_CN: '引导',
      en_US: 'Guide',
      vi_VN: 'Hướng dẫn', // Vietnamese translation
    },
    icon: 'guide',
    path: '/guide',
  },
  {
    code: 'permission',
    label: {
      zh_CN: '权限',
      en_US: 'Permission',
      vi_VN: 'Quyền hạn', // Vietnamese translation
    },
    icon: 'permission',
    path: '/permission',
    children: [
      {
        code: 'routePermission',
        label: {
          zh_CN: '路由权限',
          en_US: 'Route Permission',
          vi_VN: 'Quyền truy cập đường dẫn', // Vietnamese translation
        },
        path: '/permission/route',
      },
      {
        code: 'notFound',
        label: {
          zh_CN: '404',
          en_US: '404',
          vi_VN: '404', // No translation needed
        },
        path: '/permission/404',
      },
    ],
  },
  {
    code: 'component',
    label: {
      zh_CN: '组件',
      en_US: 'Component',
      vi_VN: 'Thành phần', // Vietnamese translation
    },
    icon: 'permission',
    path: '/component',
    children: [
      {
        code: 'componentForm',
        label: {
          zh_CN: '表单',
          en_US: 'Form',
          vi_VN: 'Biểu mẫu', // Vietnamese translation
        },
        path: '/component/form',
      },
      {
        code: 'componentTable',
        label: {
          zh_CN: '表格',
          en_US: 'Table',
          vi_VN: 'Bảng', // Vietnamese translation
        },
        path: '/component/table',
      },
      {
        code: 'componentSearch',
        label: {
          zh_CN: '查询',
          en_US: 'Search',
          vi_VN: 'Tìm kiếm', // Vietnamese translation
        },
        path: '/component/search',
      },
      {
        code: 'componentAside',
        label: {
          zh_CN: '侧边栏',
          en_US: 'Aside',
          vi_VN: 'Bên cạnh', // Vietnamese translation
        },
        path: '/component/aside',
      },
      {
        code: 'componentTabs',
        label: {
          zh_CN: '选项卡',
          en_US: 'Tabs',
          vi_VN: 'Thẻ', // Vietnamese translation
        },
        path: '/component/tabs',
      },
      {
        code: 'componentRadioCards',
        label: {
          zh_CN: '单选卡片',
          en_US: 'Radio Cards',
          vi_VN: 'Thẻ Radio', // Vietnamese translation
        },
        path: '/component/radio-cards',
      },
    ],
  },
  {
    code: 'business',
    label: {
      zh_CN: '业务',
      en_US: 'Business',
      vi_VN: 'Kinh doanh', // Vietnamese translation
    },
    icon: 'permission',
    path: '/business',
    children: [
      {
        code: 'basic',
        label: {
          zh_CN: '基本',
          en_US: 'Basic',
          vi_VN: 'Cơ bản', // Vietnamese translation
        },
        path: '/business/basic',
      },
      {
        code: 'withSearch',
        label: {
          zh_CN: '带查询',
          en_US: 'With Search',
          vi_VN: 'Có tìm kiếm', // Vietnamese translation
        },
        path: '/business/with-search',
      },
      {
        code: 'withAside',
        label: {
          zh_CN: '带侧边栏',
          en_US: 'With Aside',
          vi_VN: 'Có bên cạnh', // Vietnamese translation
        },
        path: '/business/with-aside',
      },
      {
        code: 'withRadioCard',
        label: {
          zh_CN: '带单选卡片',
          en_US: 'With Nav Tabs',
          vi_VN: 'Có thẻ điều hướng', // Vietnamese translation
        },
        path: '/business/with-radio-cards',
      },
      {
        code: 'withTabs',
        label: {
          zh_CN: '带选项卡',
          en_US: 'With Tabs',
          vi_VN: 'Có thẻ', // Vietnamese translation
        },
        path: '/business/with-tabs',
      },
    ],
  },
];

export default adminMenu;