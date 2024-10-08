import { PaginationResponse } from '@/interface/business';
import type { IForgotPasswordDto, IResetPasswordDto, LoginParams, LoginResult, LogoutResult } from '../interface/user/login';
import { request } from './base/request';
import { IRegisterFormValues } from '@/pages/register';

/** 登录接口 */
export const apiLogin = (data: LoginParams) => request<LoginResult>('post', '/account/authenticate', data);

/** 登出接口 */
export const apiLogout = () => request<LogoutResult>('post', '/account/logout');

export const apiRegisterShop = (data: IRegisterFormValues) => request<string>('post', '/account/register', data);

export const apiResetPassword = (data: IResetPasswordDto) => request<string>('put', '/account/resetpassword', data);

export const apiForgotPassword = (data: IForgotPasswordDto) => request<string>('post', '/account/forgotpassword', data);
