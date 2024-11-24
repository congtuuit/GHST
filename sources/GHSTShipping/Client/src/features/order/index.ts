import store from '@/stores';
import { removeConfirmOrderToQueue } from './orderSlice';
import { apiConfirmOrderGhn, apiGetOrderDetail } from '@/api/business.api';
import { message } from 'antd';
import { IOrderDetail, IOrderItemDto, IOrderViewDto } from '@/interface/order/order.interface';

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


export const executeHandleEditOrder = async (record: IOrderViewDto, callback: any) => {
  const orderId = record.id;
  const response = await apiGetOrderDetail(orderId, record.shopId);
  if (response.success) {
    const o: IOrderDetail = response.data;
    const editOrderObject = {
      id: o.id,
      deliveryPricePlaneId: o.deliveryPricePlaneId,
      pick_shift: o.pickShift,
      service_type_id: o.serviceTypeId,
      from_phone: o.fromPhone,
      from_name: o.fromName,
      from_province_id: o.fromProvinceId,
      from_province_name: o.fromProvinceName,
      from_address: o.fromAddress,
      from_district_id: o.fromDistrictId,
      from_district_name: o.fromDistrictName,
      from_ward_id: o.fromWardId,
      from_ward_name: o.fromWardName,

      to_phone: o.toPhone,
      to_name: o.toName,
      to_province_id: o.toProvinceId,
      to_province_name: o.toProvinceName,
      to_address: o.toAddress,
      to_district_id: o.toDistrictId,
      to_district_name: o.toDistrictName,
      to_ward_id: o.toWardId,
      to_ward_name: o.toWardName,

      weight: o.weight,
      length: o.length,
      width: o.width,
      height: o.height,
      cod_amount: o.codAmount,
      insurance_value: o.insuranceValue,
      required_note: o.requiredNote,
      note: o.note,
      payment_type_id: o.paymentTypeId,
      cod_failed_amount: o.codFailedAmount,
      items: o.items?.map((i: IOrderItemDto) => {
        return {
          ...i,
        };
      }),
    };

    localStorage.setItem(`${editOrderObject.id}`, JSON.stringify(editOrderObject));
    callback && callback();
  }
};