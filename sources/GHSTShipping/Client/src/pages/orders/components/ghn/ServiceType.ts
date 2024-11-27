export const ServiceType = {
  HangNhe: 2,
  HangNang: 5,
};

export type ServiceTypeValue = (typeof ServiceType)[keyof typeof ServiceType];
