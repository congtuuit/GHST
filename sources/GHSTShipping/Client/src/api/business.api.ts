import type { PageData } from '@/interface';
import type { BuniesssUser, PaginationResponse, ShopPricePlanDto } from '@/interface/business';

import { request } from './base/request';

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
