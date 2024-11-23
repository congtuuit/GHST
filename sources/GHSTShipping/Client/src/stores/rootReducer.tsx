import { combineReducers } from '@reduxjs/toolkit';

import userReducer from './user.store';
import tagsViewReducer from './tags-view.store';
import globalReducer from './global.store';
import orderReducer from '@/features/order/orderSlice';
import orderTempReducer from '@/features/order/orderTempSlice';

const rootReducer = combineReducers({
  user: userReducer,
  tagsView: tagsViewReducer,
  global: globalReducer,
  order: orderReducer,
  orderTemp: orderTempReducer
});

export default rootReducer;
