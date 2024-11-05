import { GHN_OrderStatus } from './contants';

type filterStatusOption = {
  name: string;
  total: number;
  children: string[];
};

class ghnOrderFilter {
  status: string[] = Object.values(GHN_OrderStatus);
  filterStatus: filterStatusOption[] = [
    {
      name: 'Nháp',
      total: 0,
      children: ['draft'],
    },
    {
      name: 'Chờ bàn giao',
      total: 0,
      children: ['ready_to_pick', 'picking', 'money_collect_picking'],
    },
    {
      name: 'Đã bàn giao - Đang giao',
      total: 0,
      children: ['picked', 'sorting', 'storing', 'transporting', 'delivering', 'delivery_fail', 'money_collect_delivering'],
    },
    {
      name: 'Đã bàn giao - đang hoàn hàng',
      total: 0,
      children: ['return', 'returning', 'return_fail', 'return_transporting', 'return_sorting'],
    },
    {
      name: 'Chờ xác nhận giao lại',
      total: 0,
      children: ['waiting_to_return'],
    },
    {
      name: 'Hoàn tất',
      total: 0,
      children: ['returned', 'delivered'],
    },
    {
      name: 'Đơn hủy',
      total: 0,
      children: ['cancel'],
    },
    {
      name: 'Hoàng thất lạc - hử hỏng',
      total: 0,
      children: ['lost', 'damage'],
    },
  ];
}


export default ghnOrderFilter