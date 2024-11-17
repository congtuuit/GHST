export interface IOrderDto {
  id: string;
  no: number;
  clientOrderCode: string;
  isPublished: boolean;
  fromAddress: string;
  toAddress: string;
  weight: number;
  paymentTypeId: number;
  insuranceValue: number;
  codAmount: number;
  publishDate: string;
  privateOrderCode: string;
  orderCode: string;
  created: Date;
}

export interface IOrderDetail extends IOrderViewDto {
  privateOrderCode: string;
}

export interface ShopOrderViewDto {
  id: string;
  uniqueCode: string;
  name: string;
  address: string;
  totalDraftOrder: number;
  totalPublishedOrder: number;
}

export interface IOrderViewDto {
  no: number;
  isPublished: boolean;
  publishDate?: Date;

  id: string;
  shopId?: string;
  shopName: string;
  deliveryPartner: string;
  deliveryFee: number;

  clientOrderCode: string;
  fromName: string;
  fromPhone: string;
  fromAddress: string;
  fromWardName: string;
  fromDistrictName: string;
  fromProvinceName: string;

  toName: string;
  toPhone: string;
  toAddress: string;
  toWardName: string;
  toDistrictName: string;
  toProvinceName: string;

  rootWeight: number;
  rootLength: number;
  rootWidth: number;
  rootHeight: number;

  weight: number;
  length: number;
  width: number;
  height: number;

  convertedWeight: number;
  calculateWeight: number;

  codAmount: number;
  insuranceValue: number;

  serviceTypeId: number;
  serviceTypeName: string;

  paymentTypeId: number;
  paymentTypeName: string;
  status: string;
  statusName: string;
  statusColor: string;
  created: string;

  totalServiceFee: number;
  insuranceFee: number;
}

export interface IUpdateOrderRequest {
  orderId?: string;
  weight?: number;
  length?: number;
  width?: number;
  height?: number;
  convertRate?: number;
}
