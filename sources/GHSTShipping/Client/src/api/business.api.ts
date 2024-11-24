import type { IPaginationRequestParameter, IShopOrderParameter, PageData } from '@/interface';
import type {
  BuniesssUser,
  ICreateDeliveryOrderRequest,
  IOrderPagedParameter,
  IPaginationResponse,
  IPickShift,
  PaginationResponse,
  ShopPricePlanDto,
} from '@/interface/business';
import type { IOrderDetail, IOrderDto, IOrderViewDto, IUpdateOrderWeightRequest, ShopOrderViewDto } from '@/interface/order/order.interface';
import type { IOrderMetadata, IShopViewDetailDto, IUpdateShopDeliveryConfigRequest } from '@/interface/shop';
import type { IDeliveryConfigDto } from '@/pages/delivery-config';

import { buildQueryString } from '@/utils/queryEncode';

import { request } from './base/request';
import { IOrder } from '@/features/order/type';
import { IChangeOperationConfig } from './type';

export const apiGetProvinces = () => request<any[]>('get', '/metadata/provinces');
export const apiGetDictricts = () => request<any[]>('get', '/metadata/dictricts');
export const apiGetWards = (districtId: number) => request<any[]>('get', '/metadata/wards?districtId=' + districtId);
export const apiGetPickShifts = () => request<IPickShift[]>('get', '/delivery/pickshifts');
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

export const apiDeleteShopPricePlan = (id: string, ids: string[] = []) => {
  return request('delete', `/shops/prices/${id}`, ids);
};

//ICreateDeliveryOrderRequest
export const apiCreateDeliveryOrder = (data: IOrder) => {
  return request('post', '/orders/ghn/create', data);
};

//ICreateDeliveryOrderRequest
export const apiUpdateDeliveryOrder = (id: string, data: IOrder) => {
  return request('post', `/orders/ghn/update/${id}`, data);
};

export const apiGetOrders = (params: IOrderPagedParameter) => {
  // Build query string
  const queryString = buildQueryString(params);

  return request<IPaginationResponse<IOrderViewDto>>('get', `/orders/list?${queryString}`);
};

export const apiGetOrderDetail = (orderId: string | undefined) => {
  return request<IOrderDetail>('get', `/orders/ghn/detail/${orderId}`);
};

export const apiCancelOrderGhn = (orderIds: string[]) => {
  return request<IOrderDetail>('put', `/orders/ghn/cancel`, {
    orderIds: orderIds,
  });
};

export const apiGetShops = (pageNumber: number | undefined = 1, pageSize: number | undefined = 10) => {
  return request<PaginationResponse>('get', `/users/shops?pageNumber=${pageNumber}&pageSize=${pageSize}`);
};

export const apiActiveShops = (shopId: string) => request<PaginationResponse>('put', `/users/activeshop/${shopId}`);

export const apiGetShopDetail = (shopId: string) => request<IShopViewDetailDto>('get', `/shops/detail/${shopId}`);
export const apiChangeOperationConfig = (payload: IChangeOperationConfig) =>
  request<IShopViewDetailDto>('put', `/shops/changeOperationConfig/${payload.shopId}`, payload);
export const apiUpdateGhnShopId = (shopId: string, ghnShopId: number) =>
  request('put', `/shops/ghnShopId/${shopId}`, {
    shopId: shopId,
    ghnShopId: ghnShopId,
  });

export const apiGetDeliveryConfigs = () => request<IDeliveryConfigDto[]>('get', `/configs/delivery`);
export const apiUpdateDeliveryConfigs = (payload: IDeliveryConfigDto[]) => request('put', `/configs/delivery`, payload);
export const apiCreateDeliveryConfig = (payload: any) => request('post', `/configs/delivery`, payload);

export const apiUpdateShopDeliveryConfig = (payload: IUpdateShopDeliveryConfigRequest) => request('put', `/configs/shop`, payload);
export const apiGetOrderMetaData = () => request<IOrderMetadata>('get', `/orders/metadata`);
export const apiGetShopOrders = (params: IShopOrderParameter) => {
  const queryString = buildQueryString(params);
  return request<IPaginationResponse<ShopOrderViewDto>>('get', `/orders/group-by-shops?${queryString}`);
};
export const apiConfirmOrderGhn = (orderId: string) => request('put', `/orders/ghn/confirm/${orderId}`);
export const apiCountOrderByStatus = (shopId: string) => request('get', `/orders/ghn/count-order-by-status/${shopId}`);
export const apiUpdateGhnOrderWeight = (payload: IUpdateOrderWeightRequest) => request('put', `/orders/ghn/update`, payload);
