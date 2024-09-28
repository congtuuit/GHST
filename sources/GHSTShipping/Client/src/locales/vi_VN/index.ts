import { viVN_account } from './account';
import { vi_VN_component } from './component';
import { viVN_dashboard } from './dashboard';
import { vi_VN_documentation } from './documentation';
import { viVN_globalTips } from './global/tips';
import { viVN_guide } from './guide';
import { viVN_notice } from './notice';
import { viVN_permissionRole } from './permission/role';
import { viVN_avatorDropMenu } from './user/avatorDropMenu';
import { viVN_tagsViewDropMenu } from './user/tagsViewDropMenu';
import { viVN_title } from './user/title';

const vi_VN = {
  ...viVN_account,
  ...viVN_avatorDropMenu,
  ...viVN_tagsViewDropMenu,
  ...viVN_title,
  ...viVN_globalTips,
  ...viVN_permissionRole,
  ...viVN_dashboard,
  ...viVN_guide,
  ...vi_VN_component,
  ...viVN_notice,
  ...vi_VN_documentation,
};

export default vi_VN;
