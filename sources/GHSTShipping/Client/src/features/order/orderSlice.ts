// store/orderSlice.ts
import { ICreateDeliveryOrderRequest } from '@/interface/business';
import { createSlice, PayloadAction } from '@reduxjs/toolkit';

interface IOrderState {
  order: ICreateDeliveryOrderRequest | null;
}

const initialState: IOrderState = {
  order: null,
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
  },
});

export const { setOrder, clearOrder } = orderSlice.actions;
export default orderSlice.reducer;
