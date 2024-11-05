export const GHN_OrderServiceType = {
  HangNhe: 2,
  HangNang: 5,
};

export const GHN_OrderPaymentType = {
  BenGuiTraPhi: 1,
  BenNhanTraPhi: 2,
};

export const GHN_OrderStatus = {
  ReadyToPick: 'ready_to_pick',
  Picking: 'picking',
  MoneyCollectPicking: 'money_collect_picking',
  Picked: 'picked',
  Sorting: 'sorting',
  Storing: 'storing',
  Transporting: 'transporting',
  Delivering: 'delivering',
  DeliveryFail: 'delivery_fail',
  MoneyCollectDelivering: 'money_collect_delivering',
  Return: 'return',
  Returning: 'returning',
  ReturnFail: 'return_fail',
  ReturnTransporting: 'return_transporting',
  ReturnSorting: 'return_sorting',
  WaitingToReturn: 'waiting_to_return',
  Returned: 'returned',
  Delivered: 'delivered',
  Cancel: 'cancel',
  Lost: 'lost',
  Damage: 'damage',
} as const;
