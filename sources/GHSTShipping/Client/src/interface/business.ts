import { IPaginationRequestParameter } from ".";

export interface BuniesssUser {
  key: string;
  firstName: string;
  lastName: string;
  age: number;
  address: string;
  tags: string[];
}

export interface PaginationResponse {
  count: number;
  pageNumber: number;
  pageSize: number;
  data: any[];
}

export interface IPaginationResponse<T> {
  count: number;
  pageNumber: number;
  pageSize: number;
  data: T[];
}

export interface IOrderPagedParameter extends IPaginationRequestParameter {
  deliveryPartner: string | undefined;
  orderCode: string | undefined;
}

export interface ShopPricePlanDto {
  id: string | undefined; // Guid in C# can be represented as a string in TypeScript
  shopId: string | undefined;
  supplier: 
    | 'GHN' 
    | 'SHOPEE EXPRESS' 
    | 'J&T' 
    | 'Best' 
    | 'Viettel' 
    | 'GHTK'; // Các giá trị cho supplier
  privatePrice: number;
  officialPrice: number;
  capacity: number;
}

export interface ICreateDeliveryOrderRequest {
  // DONE
  from_name: string;                 // Required sender name
  from_phone: string;                // Required sender phone
  from_address: string;              // Required sender address
  from_ward_name: string;            // Required sender ward name
  from_district_name: string;        // Required sender district name
  from_province_name: string;        // Required sender province name

  pick_shift: number[];              // Required list of pick shifts

  to_phone: string;                  // Required recipient phone
  to_name: string;                   // Required recipient name
  to_address: string;                // Required recipient address
  to_district_id: string;            // Required recipient district ID
  to_ward_code: string;              // Required recipient ward code

  items: DeliveryOrderItemDto[];     // Required list of delivery items

  weight?: number;                   // Optional weight
  length?: number;                   // Optional length
  width?: number;                    // Optional width
  height?: number;                   // Optional height

  cod_amount?: number;               // Optional COD amount
  insurance_value: number;           // Required insurance value

  required_note: string;             // Required note
  note?: string;                     // Optional note
  payment_type_id: number;           // Required payment type ID
  service_id: number;                // Required service ID
  service_type_id: number;           // Required service type ID

  content?: string;                  // Optional content
  pick_station_id: number;           // Required pick station ID
  deliver_station_id?: number;       // Optional deliver station ID
  coupon?: string;                   // Optional coupon
  return_phone?: string;             // Optional return phone
  return_address?: string;           // Optional return address
  return_district_id?: string;       // Optional return district ID
  return_ward_code?: string;         // Optional return ward code

  client_order_code?: string;       // Optional client order code
}

interface DeliveryOrderItemDto {
  name: string;                      // Required item name
  weight: number;
  quantity: number;                  // Required quantity
  code: string;                      // Required item code
  price: number;                     // Required price
  length: number;                    // Required length
  width: number;                     // Required width
  height: number;                    // Required height
  category: OrderCategoryDto;        // Required category
}

interface OrderCategoryDto {
  level1: string;                   // Required level 1 category
}
