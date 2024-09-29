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
}

export interface LogoutParams {
  token: string;
}

export interface LogoutResult {}
