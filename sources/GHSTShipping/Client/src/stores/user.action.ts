import type { LoginParams, LoginResult } from '../interface/user/login';
import type { Dispatch } from '@reduxjs/toolkit';

import { message as $message, message } from 'antd';

import { apiLogin, apiLogout } from '../api/auth.api';
import { setUserItem } from './user.store';
import { createAsyncAction } from './utils';

// typed wrapper async thunk function demo, no extra feature, just for powerful typings
export const loginAsync = createAsyncAction<LoginParams, boolean>(payload => {
  return async dispatch => {
    const _result = await apiLogin(payload);
    const { success, data, message } = _result;

    if (success) {
      if (!data.isVerified) {
        $message.info('Tài khoản của bạn chưa được kích hoạt, vui lòng liên hệ Admin!');

        return false;
      }

      localStorage.setItem('t', data.jwToken);
      localStorage.setItem('userName', data.userName);
      localStorage.setItem('session', JSON.stringify(data));
      dispatch(
        setUserItem({
          logged: true,
          userName: data.userName,
          session: data,
        }),
      );

      return true;
    } else {
      if (Boolean(message)) {
        $message.error(message);
      } else {
        $message.error('Tài khoản hoặc mật khẩu không đúng!');
      }
    }

    return false;
  };
});

export const logoutAsync = () => {
  return async (dispatch: Dispatch) => {
    await apiLogout();
    localStorage.clear();
    dispatch(
      setUserItem({
        logged: false,
      }),
    );

    return true;
  };
};
