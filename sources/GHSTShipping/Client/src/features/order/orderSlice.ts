// store/orderSlice.ts
import { ICreateDeliveryOrderRequest } from '@/interface/business';
import { ShopOrderViewDto } from '@/interface/order/order.interface';
import { createSlice, PayloadAction } from '@reduxjs/toolkit';

interface IOrderState {
  order: ICreateDeliveryOrderRequest | null;
  orders: any[];
  shop: ShopOrderViewDto | null;
}

const initialState: IOrderState = {
  order: null,
  orders: [],
  shop: null
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
    }
  },
});

export const { setOrder, clearOrder, setShopInfo } = orderSlice.actions;
export default orderSlice.reducer;
