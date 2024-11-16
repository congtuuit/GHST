import { request } from '@/api/base/request';

export interface BasicShopInfoDto {
  id?: string;
  name: string;
  address: string;
  phoneNumber: string;
  wardId: string;
  wardName: string;
  districtId: string;
  districtName: string;
  provinceId: string;
  provinceName: string;
}

export const getShops = (shopId: string = '') => {
  return request<BasicShopInfoDto[]>('get', `/shops/List?id=${shopId}`);
};

export const createShop = async (shop: BasicShopInfoDto) => {
  return request<string>('post', `/shops/create`, shop);
};