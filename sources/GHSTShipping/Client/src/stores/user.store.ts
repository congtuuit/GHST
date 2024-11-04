import type { LoginResult, Role } from '@/interface/user/login';
import type { Locale, UserState } from '@/interface/user/user';
import type { PayloadAction } from '@reduxjs/toolkit';
import { createSlice } from '@reduxjs/toolkit';
import { getGlobalState } from '@/utils/getGloabal';
import { IStore } from './types';

const initialState: UserState = {
  ...getGlobalState(),
  noticeCount: 0,
  locale: (localStorage.getItem('locale')! || 'vi_VN') as Locale,
  //newUser: JSON.parse(localStorage.getItem('newUser')!) ?? true,
  newUser: false,
  logged: localStorage.getItem('t') ? true : false,
  menuList: [],
  userName: localStorage.getItem('userName') || '',
  session: (JSON.parse(localStorage.getItem('session') ?? 'null') || null) as LoginResult,
};

const userSlice = createSlice({
  name: 'user',
  initialState,
  reducers: {
    setUserItem(state, action: PayloadAction<Partial<UserState>>) {
      const { userName, session } = action.payload;
      if (userName !== state.userName) {
        localStorage.setItem('userName', action.payload.userName || '');
      }

      Object.assign(state, action.payload);
    },
  },
});

export const { setUserItem } = userSlice.actions;

export const rolesSelector = (state: IStore) => state.user.session.roles ?? [];

export default userSlice.reducer;
