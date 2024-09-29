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
