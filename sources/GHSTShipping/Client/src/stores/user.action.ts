import type { LoginParams, LoginResult } from '../interface/user/login';
import type { Dispatch } from '@reduxjs/toolkit';
import { message as $message, message } from 'antd';
import { apiLogin, apiLogout } from '../api/user.api';
import { setUserItem } from './user.store';
import { createAsyncAction } from './utils';

// typed wrapper async thunk function demo, no extra feature, just for powerful typings
export const loginAsync = createAsyncAction<LoginParams, boolean>(payload => {
  return async dispatch => {
    const _result = await apiLogin(payload);
    const { success, data } = _result;
    if (success) {
      localStorage.setItem('t', data.jwToken);
      localStorage.setItem('userName', data.userName);
      localStorage.setItem('session', JSON.stringify(data));
      dispatch(
        setUserItem({
          logged: true,
          username: data.userName,
        }),
      );

      return true;
    }

    $message.error('Tài khoản hoặc mật khẩu không đúng');

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
