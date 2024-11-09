// store/orderSlice.ts
import { ICreateDeliveryOrderRequest } from '@/interface/business';
import { ShopOrderViewDto } from '@/interface/order/order.interface';
import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { IOrderFilter } from './type';

export interface IOrderState {
  order: ICreateDeliveryOrderRequest | null;
  orders: any[];
  shop: ShopOrderViewDto | null;
  orderFilter?: IOrderFilter;
  confirmOrderQueue: string[];
}

const initialState: IOrderState = {
  order: null,
  orders: [],
  shop: null,
  confirmOrderQueue: [],
};

const orderSlice = createSlice({
  name: 'order',
  initialState,
  reducers: {
    setOrder(state, action: PayloadAction<ICreateDeliveryOrderRequest>) {
      state.order = action.payload;
    },
    clearOrder(state) {
      state.order = null;
    },
    setShopInfo(state, action: PayloadAction<ShopOrderViewDto>) {
      state.shop = action.payload;
    },
    setOrderFilter(state, action: PayloadAction<IOrderFilter>) {
      state.orderFilter = {
        ...state.orderFilter,
        ...action.payload,
      };
    },
    addConfirmOrderToQueue(state, action: PayloadAction<string[]>) {
      let _array = [...state.confirmOrderQueue, ...action.payload]
      const uniqueArray = [...new Set(_array)];
      state.confirmOrderQueue = [...uniqueArray];
    },
    removeConfirmOrderToQueue(state, action: PayloadAction<string>) {
      const values = state.confirmOrderQueue.filter(item => item !== action.payload);
      state.confirmOrderQueue = [...values];
    },
  },
});

export const { setOrder, clearOrder, setShopInfo, setOrderFilter, addConfirmOrderToQueue, removeConfirmOrderToQueue } = orderSlice.actions;
export default orderSlice.reducer;
