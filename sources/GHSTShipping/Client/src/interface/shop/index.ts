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
  ghnShopDetails: IGhnShopDetailDto[];
  ghnShopId?: number;
}

interface IGhnShopDetailDto {
  id: number;
  name: string;
  displayName: string;
}
