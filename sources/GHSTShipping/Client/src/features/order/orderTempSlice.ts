// store/orderSlice.ts
import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { IOrderFilter } from './type';

export interface IOrderTempState {
  editOrder: any;
  orderFilter?: IOrderFilter;
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
    setOrderFilter(state, action: PayloadAction<IOrderFilter>) {
      state.orderFilter = {
        ...state.orderFilter,
        ...action.payload,
      };
    },
  },
});


export const { setEditOrder, setOrderFilter } = orderTempSlice.actions;


export default orderTempSlice.reducer;
