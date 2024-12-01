import { ICacheAddressInfo } from './type';

class AddressCache {
  private cache: Map<string, Map<string, ICacheAddressInfo>>;

  constructor() {
    this.cache = new Map();
    this.loadFromLocalStorage();
  }

  // Hàm lưu thông tin vào cache và đồng bộ với localStorage
  save(address: ICacheAddressInfo): void {
    const { shopId, to_phone } = address;

    // Nếu shopId chưa có trong cache, thêm Map mới
    if (!this.cache.has(shopId)) {
      this.cache.set(shopId, new Map());
    }

    // Lưu thông tin với key là to_phone
    this.cache.get(shopId)?.set(to_phone, address);

    // Đồng bộ với localStorage
    this.syncToLocalStorage();
  }

  // Hàm lấy thông tin dựa trên shopId và to_phone
  get(shopId: string, to_phone: string): ICacheAddressInfo | null {
    const shopCache = this.cache.get(shopId);
    return shopCache?.get(to_phone) || null;
  }

  // Hàm xóa thông tin khỏi cache và đồng bộ với localStorage
  remove(shopId: string, to_phone: string): boolean {
    const shopCache = this.cache.get(shopId);
    if (shopCache?.has(to_phone)) {
      shopCache.delete(to_phone);

      // Nếu không còn thông tin nào trong shopId, xóa shopId
      if (shopCache.size === 0) {
        this.cache.delete(shopId);
      }

      // Đồng bộ với localStorage
      this.syncToLocalStorage();
      return true;
    }
    return false;
  }

  // Đồng bộ cache với localStorage
  private syncToLocalStorage(): void {
    const cacheObj: { [key: string]: { [key: string]: ICacheAddressInfo } } = {};

    // Chuyển đổi Map thành object để lưu vào localStorage
    this.cache.forEach((shopCache, shopId) => {
      cacheObj[shopId] = {};
      shopCache.forEach((address, to_phone) => {
        cacheObj[shopId][to_phone] = address;
      });
    });

    // Lưu dữ liệu vào localStorage
    localStorage.setItem('addressCache', JSON.stringify(cacheObj));
  }

  // Tải dữ liệu từ localStorage
  private loadFromLocalStorage(): void {
    const cacheData = localStorage.getItem('addressCache');
    if (cacheData) {
      const parsedData = JSON.parse(cacheData);

      // Chuyển dữ liệu từ object vào Map
      Object.keys(parsedData).forEach((shopId) => {
        const shopCache = new Map<string, ICacheAddressInfo>();
        const addresses = parsedData[shopId];
        Object.keys(addresses).forEach((to_phone) => {
          shopCache.set(to_phone, addresses[to_phone]);
        });
        this.cache.set(shopId, shopCache);
      });
    }
  }
}

export default AddressCache;
