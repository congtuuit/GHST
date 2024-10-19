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
