import { GHN_OrderStatus } from './contants';
import { IDeliveryStatus } from './type';

const OrderGroupStatus = {
  Nhap: 0,
  ChoBanGiao: 1,
  DaBanGiaoDangGiao: 2,
  DaBanGiaoDangHoangHang: 3,
  ChoXacNhanGiaoLai: 4,
  HoanTat: 5,
  DaHuy: 6,
  ThatLacHong: 7,
}

export type FilterStatusOption = {
  name: string;
  value: number;
  total: number;
  children: (keyof IDeliveryStatus)[];
};

class ghnOrderFilter {
  status: string[] = Object.values(GHN_OrderStatus);
  filterStatus: FilterStatusOption[] = [
    {
      name: 'Nháp',
      value: OrderGroupStatus.Nhap,
      total: 0,
      children: ['draft'],
    },
    {
      name: 'Chờ bàn giao',
      value: OrderGroupStatus.ChoBanGiao,
      total: 0,
      children: ['ready_to_pick', 'picking', 'money_collect_picking'],
    },
    {
      name: 'Đã bàn giao - Đang giao',
      value: OrderGroupStatus.DaBanGiaoDangGiao,
      total: 0,
      children: ['picked', 'sorting', 'storing', 'transporting', 'delivering', 'delivery_fail', 'money_collect_delivering'],
    },
    {
      name: 'Đã bàn giao - đang hoàn hàng',
      value: OrderGroupStatus.DaBanGiaoDangHoangHang,
      total: 0,
      children: ['return', 'returning', 'return_fail', 'return_transporting', 'return_sorting'],
    },
    {
      name: 'Chờ xác nhận giao lại',
      value: OrderGroupStatus.ChoXacNhanGiaoLai,
      total: 0,
      children: ['waiting_to_return'],
    },
    {
      name: 'Hoàn tất',
      value: OrderGroupStatus.HoanTat,
      total: 0,
      children: ['returned', 'delivered'],
    },
    {
      name: 'Đơn hủy',
      value: OrderGroupStatus.DaHuy,
      total: 0,
      children: ['cancel'],
    },
    {
      name: 'Thất lạc / Hư hỏng',
      value: OrderGroupStatus.ThatLacHong,
      total: 0,
      children: ['lost', 'damage'],
    },
  ];

  reCalculateFilterStatusTotals = (status: IDeliveryStatus) => {
    console.log("status[key] ", status)

    return this.filterStatus.map(category => {
      return {
        ...category,
        total: category.children.reduce((sum, key) => sum + (status[key] || 0), 0),
      };
    });
  };
}

export default ghnOrderFilter;
