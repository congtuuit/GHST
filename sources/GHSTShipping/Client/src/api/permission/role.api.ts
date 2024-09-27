import type { GetRoleResult } from '@/interface/permission/role.interface';

import { request } from '../base/request';

/** get role list api */
export const apiGetRoleList = () => request<GetRoleResult>('get', '/permission/role');
