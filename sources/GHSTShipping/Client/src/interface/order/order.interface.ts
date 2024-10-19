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
}

export interface IOrderDetail {
  id: string;
  privateOrderCode: string;
  status: string;
  clientOrderCode: string;
}
