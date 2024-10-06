export interface IOrderStatus {
  code: string;
  name: string;
}

export interface IOrderStatuses {
  [supplier: string]: IOrderStatus[];
}

export const orderStatuses: IOrderStatuses = {
  GHN: [
    {
      code: 'DRAFT',
      name: 'Đơn nháp',
    } ,
    {
      code: 'WAIT_CONFIRM',
      name: 'Chờ bàn giao',
    } ,
    {
      code: 'DELIVERING',
      name: 'Đã bàn giao - Đang giao',
    } ,
    {
      code: 'RETURN',
      name: 'Đã bàn giao - Đang hoàn hàng',
    } ,
    {
      code: 'WAIT_CONFIRM_DELIVERY',
      name: 'Chờ xác nhận giao lại',
    } ,
    {
      code: 'COMPLETED',
      name: 'Hoàn tất',
    } ,
    {
      code: 'CANCEL',
      name: 'Đơn hủy',
    } ,
    {
      code: 'LOST',
      name: 'Thất lạc - hư hỏng',
    } ,
  ] as IOrderStatus[],
};
