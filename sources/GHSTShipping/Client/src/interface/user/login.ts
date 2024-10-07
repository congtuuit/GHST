/** user's role */
export type Role = 'SHOP' | 'ADMIN';

export interface LoginParams {
  /** 用户名 */
  username: string;
  /** 用户密码 */
  password: string;
}

export interface LoginResult {
  /** auth token */
  id: string;
  userName: string;
  email: string;
  roles: string[];
  isVerified: boolean;
  jwToken: string;
  phoneNumber: string;
  fullName: string;
}

export interface LogoutParams {
  token: string;
}

export interface LogoutResult {}

export interface IResetPasswordDto {
  token: string;
  email: string;
  password: string;
  confirmPassword: string;
}

export interface IForgotPasswordDto {
  email: string;
  phoneNumber: string;
}
