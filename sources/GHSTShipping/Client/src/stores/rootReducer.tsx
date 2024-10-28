import { combineReducers } from '@reduxjs/toolkit';

import globalReducer from './global.store';
import tagsViewReducer from './tags-view.store';
import userReducer from './user.store';
import orderReducer from '@/features/order/orderSlice';


const rootReducer = combineReducers({
  user: userReducer,
  tagsView: tagsViewReducer,
  global: globalReducer,
  order: orderReducer,
});

export default rootReducer;
