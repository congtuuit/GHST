import { message } from 'antd';

export const commingSoon = () => {
  console.log('helo');
  message.info('Tính năng đang phát triển...');
};

export function debounce<Params extends any[]>(func: (...args: Params) => any, timeout: number): (...args: Params) => void {
  let timer: NodeJS.Timeout;

  return (...args: Params) => {
    clearTimeout(timer);
    timer = setTimeout(() => {
      func(...args);
    }, timeout);
  };
}

// Function to set an item in local storage with expiration
export function setItemWithExpiry(key: string, value: any, ttl: number) {
  const now = new Date();

  // Create an object to store the value and its expiration time
  const item = {
      value: value,
      expiry: now.getTime() + ttl, // Expiration time in milliseconds
  };

  // Store the item in local storage
  localStorage.setItem(key, JSON.stringify(item));
}

// Function to get an item from local storage
export function getItemWithExpiry(key: string) {
  const itemStr = localStorage.getItem(key);

  // If the item doesn't exist, return null
  if (!itemStr) {
      return null;
  }

  const item = JSON.parse(itemStr);
  const now = new Date();

  // Compare the expiration time with the current time
  if (now.getTime() > item.expiry) {
      // Item has expired, remove it from local storage
      localStorage.removeItem(key);
      return null;
  }

  return item.value;
}
