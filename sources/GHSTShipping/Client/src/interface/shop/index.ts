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
  status: string;
  ghnShopDetails?: { [id: string]: IGhnShopDetailDto[] };
  ghnShopId?: number;

  partners?: IDeliveryParter[];
  shopConfigs?: IShopConfig[];
}

export interface IGhnShopDetailDto {
  id: number;
  name: string;
  displayName: string;
  phone: string;
  address: string;
  wardCode: string;
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
}

interface IShopDeliveryConfigDto {
  deliveryPartner: number;
  deliveryPartnerName: string;
  shops: IGhnShopDetailDto[];
}