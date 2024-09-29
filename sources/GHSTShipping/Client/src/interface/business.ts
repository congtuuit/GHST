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

