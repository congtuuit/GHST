import type { LoginParams, LoginResult, LogoutParams, LogoutResult } from '../interface/user/login';

import { request } from './base/request';

/** 登录接口 */
export const apiLogin = (data: LoginParams) => request<LoginResult>('post', '/Account/Authenticate', data);

/** 登出接口 */
export const apiLogout = () => request<LogoutResult>('post', '/Account/Logout');
