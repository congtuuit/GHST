// store/orderSlice.ts
import { ICreateDeliveryOrderRequest } from '@/interface/business';
import { createSlice, PayloadAction } from '@reduxjs/toolkit';

interface IOrderState {
  order: ICreateDeliveryOrderRequest | null;
  orders: any[];
  shopId: string | null;
}

const initialState: IOrderState = {
  order: null,
  orders: [],
  shopId: null
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
    setShopId(state, action: PayloadAction<string>) {
      state.shopId = action.payload;
    }
  },
});

export const { setOrder, clearOrder, setShopId } = orderSlice.actions;
export default orderSlice.reducer;
