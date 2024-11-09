import store from '@/stores';
import { removeConfirmOrderToQueue } from './orderSlice';
import { apiConfirmOrderGhn } from '@/api/business.api';
import { message } from 'antd';

declare global {
  interface Window {
    handleCallApiConfirmOrder?: ReturnType<typeof setTimeout>;
  }
}

export const handleCallApiConfirmOrder = async (callback: any) => {
  // Xóa timeout nếu hàm đã được gọi trước đó
  if (window.handleCallApiConfirmOrder) {
    clearTimeout(window.handleCallApiConfirmOrder);
  }

  window.handleCallApiConfirmOrder = setTimeout(async () => {
    const { order } = store.getState();

    if (order?.confirmOrderQueue?.length > 0) {
      const [firstOrder] = order.confirmOrderQueue;
      const response = await apiConfirmOrderGhn(firstOrder);
      if (response.success) {
        message.success(`Xác nhận đơn hàng thành công`);
      }

      // Dispatch action để xóa đơn hàng đầu tiên khỏi hàng đợi
      store.dispatch(removeConfirmOrderToQueue(firstOrder));

      // Gọi lại hàm đệ quy
      await handleCallApiConfirmOrder(callback);
    } else {
      callback && callback();
    }
  }, 100);
};
