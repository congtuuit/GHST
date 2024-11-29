import { BasicShopInfoDto } from '@/features/shop';

export interface IShopViewDetailDto {
  id: string;
  shopUniqueCode: string;
  registerDate: string | null;
  shopName: string;
  fullName: string;
  email: string;
  avgMonthlyCapacity: number;
  isVerified: boolean;
  accountId: string;
  phoneNumber: string;
  bankName: string;
  bankAccountNumber: string;
  bankAccountHolder: string;
  bankAddress: string;
  allowPublishOrder: boolean;
  allowUsePartnerShopAddress: boolean;
  status: string;
  ghnShopDetails?: { [id: string]: IGhnShopDetailDto[] };
  ghnShopId?: number;

  partners?: IDeliveryParter[];
  shopConfigs?: IShopConfig[];
}

export interface IGhnShopDetailDto {
  id: string;
  name: string;
  displayName: string;
  phone: string;
  address: string;
  wardCode: string;
  wardName: string;
  districtName: string;
  provineName: string;
  districtId: string;
}

export interface IUpdateShopDeliveryConfigRequest {
  shopId?: string;
  deliveryConfigId: string;
  partnerShopId: string;
  isConnect: boolean;
  clientPhone: string;
}

export interface IShopConfig {
  partnerName: string;
  partnerAccountName: string;
  partnerConfigId: string;
  partnerShopId?: string;
  clientPhone?: string;

  address?: string;
  wardName?: string;
  districtName?: string;
  proviceName?: string;
}

export interface IDeliveryParter {
  partnerName: string;
  partnerAccountName: string;
  partnerConfigId: string;
}

export interface IOrderMetadata {
  deliveryConfigs: IShopDeliveryConfigDto[];
  deliveryPricePlanes: DeliveryPricePlaneFormDto[];
  myShops: BasicShopInfoDto[];
}

interface IShopDeliveryConfigDto {
  deliveryPartner: number;
  deliveryPartnerName: string;
  shops: IGhnShopDetailDto[];
}

export interface DeliveryPricePlaneFormDto {
  id?: string;
  shopId?: string;
  name: string;
  minWeight: number;
  maxWeight: number;
  publicPrice: number;
  privatePrice: number;
  stepPrice: number;
  stepWeight: number;
  limitInsurance: number;
  insuranceFeeRate: number;
  returnFeeRate: number;
  convertWeightRate: number;
  isActivated: boolean;

  parentId?: string;
}
