export const supplierKeys = {
  GHN: 'GHN',
  SHOPEE_EXPRESS: 'SHOPEE EXPRESS',
  JnT: 'J&T',
  Best: 'Best',
  Viettel: 'Viettel',
  GHTK: 'GHTK',
};

export const DeliveryParterName = {
  GHN: 'GHN',
  SHOPEE_EXPRESS: 'SHOPEE EXPRESS',
  JnT: 'J&T',
  Best: 'Best',
  Viettel: 'Viettel',
  GHTK: 'GHTK',
};

export const suppliers = [
  supplierKeys.GHN,
  supplierKeys.SHOPEE_EXPRESS,
  supplierKeys.JnT,
  supplierKeys.Best,
  supplierKeys.Viettel,
  supplierKeys.GHTK,
];

export const EnumDeliveryPartner = {
  GHN: 1,
  SPX: 2,
  JT: 3,
  Best: 4,
  Viettel: 5,
  GHTK: 6,
};

// Chuyển đối tượng EnumDeliveryPartner thành mảng
export const deliveryPartnerArray = Object.entries(EnumDeliveryPartner).map(([key, value]) => ({
  key,
  value,
}));
