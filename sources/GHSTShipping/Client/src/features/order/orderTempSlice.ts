// store/orderSlice.ts
import { ICreateDeliveryOrderRequest } from '@/interface/business';
import { IOrderViewDto, ShopOrderViewDto } from '@/interface/order/order.interface';
import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { IOrderFilter } from './type';

export interface IOrderTempState {
  editOrder: any;
}

const initialState: IOrderTempState = {
  editOrder: null,
};

const orderTempSlice = createSlice({
  name: 'orderTemp',
  initialState,
  reducers: {
    setEditOrder(state, action: PayloadAction<any>) {
      state.editOrder = action.payload;
    },
  },
});

export const { setEditOrder } = orderTempSlice.actions;
export default orderTempSlice.reducer;
