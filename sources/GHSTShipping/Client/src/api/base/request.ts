import type { AxiosRequestConfig, Method } from 'axios';

import { message as $message, message } from 'antd';
import axios from 'axios';

import store from '@/stores';
import { setGlobalState } from '@/stores/global.store';
// import { history } from '@/routes/history';

const axiosInstance = axios.create({
  timeout: 6000,
});

axiosInstance.interceptors.request.use(
  config => {
    const token = localStorage.getItem('t'); // Or get from Redux store using store.getState()
    if (token) {
      // Attach the token to the Authorization header
      // Ensure config.headers is defined
      config.headers = config.headers ?? {};
      config.headers['Authorization'] = `Bearer ${token}`;
    }

    store.dispatch(
      setGlobalState({
        loading: true,
      }),
    );

    return config;
  },
  error => {
    store.dispatch(
      setGlobalState({
        loading: false,
      }),
    );
    Promise.reject(error);
  },
);

axiosInstance.interceptors.response.use(
  config => {
    store.dispatch(
      setGlobalState({
        loading: false,
      }),
    );

    if (config?.data?.message) {
      // $message.success(config.data.message)
    }

    return config?.data;
  },
  error => {
    store.dispatch(
      setGlobalState({
        loading: false,
      }),
    );

    // if needs to navigate to login page when request exception
    // history.replace('/login');
    let errorMessage = 'Xảy ra lỗi';
    if (error?.message?.includes('Network Error')) {
      errorMessage = 'Kết nối mạng gặp sự cố, vui lòng kiểm tra lại!';
    } else {
      errorMessage = error?.message;
    }

    console.dir(error);

    return {
      message: errorMessage,
      success: false,
      ...error?.response?.data,
    };
  },
);

export type Response<T = any> = {
  success: boolean;
  message: string;
  errors?: any;
  data: T;
};

export type MyResponse<T = any> = Promise<Response<T>>;

/**
 *
 * @param method - request methods
 * @param url - request url
 * @param data - request data or params
 */
export const request = <T = any>(
  method: Lowercase<Method>,
  url: string,
  data?: any,
  config?: AxiosRequestConfig,
): MyResponse<T> => {
  const apiUrl = import.meta.env.VITE_API_URL;
  const endpoint = apiUrl + url;
  if (method === 'post') {
    return axiosInstance.post(endpoint, data, config);
  }
  if (method === 'put') {
    return axiosInstance.put(endpoint, data, config);
  }
  if (method === 'delete') {
    return axiosInstance.delete(endpoint, config);
  }
  return axiosInstance.get(endpoint, {
    params: data,
    ...config,
  });
};
