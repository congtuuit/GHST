import type { PageData } from '@/interface';
import {
  IOrderPagedParameter,
  IPaginationResponse,
  type BuniesssUser,
  type ICreateDeliveryOrderRequest,
  type PaginationResponse,
  type ShopPricePlanDto,
} from '@/interface/business';

import { request } from './base/request';
import { IOrderDetail, IOrderDto } from '@/interface/order/order.interface';
import { buildQueryString } from '@/utils/queryEncode';

export const apiGetProvinces = () => request<any[]>('get', '/metadata/provinces');
export const apiGetDictricts = () => request<any[]>('get', '/metadata/dictricts');
export const apiGetWards = (districtId: number) => request<any[]>('get', '/metadata/wards?districtId=' + districtId);
export const apiGetPickShifts = () => request<any[]>('get', '/delivery/pickshifts');
export const getBusinessUserList = (params: any) => request<PageData<BuniesssUser>>('get', '/business/list', params);
export const apiGetShopPricePlanes = (
  shopId: string | undefined,
  supplier: string,
  pageNumber: number | undefined = 1,
  pageSize: number | undefined = 10,
) => {
  return request<PaginationResponse>(
    'get',
    `/shops/prices?shopId=${shopId ?? ''}&supplier=${supplier ?? ''}&pageNumber=${pageNumber}&pageSize=${pageSize}`,
  );
};
export const apiCreateShopPricePlan = (data: ShopPricePlanDto) => {
  return request('post', `/shops/prices`, data);
};
export const apiDeleteShopPricePlan = (id: string) => {
  return request('delete', `/shops/prices/${id}`);
};
export const apiCreateDeliveryOrder = (data: ICreateDeliveryOrderRequest) => {
  return request('post', '/orders/ghn/create', data);
};
export const apiGetOrders = (params: IOrderPagedParameter) => {
   // Build query string
   const queryString = buildQueryString(params);
  return request<IPaginationResponse<IOrderDto>>('get', `/orders/ghn/list?${queryString}`);
};
export const apiGetOrderDetail = (orderId: string | undefined) => {
 return request<IOrderDetail>('get', `/orders/ghn/detail/${orderId}`);
};
