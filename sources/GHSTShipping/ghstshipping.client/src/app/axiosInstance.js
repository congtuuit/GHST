// services/axiosInstance.js
import axios from 'axios';

// Create an instance of axios with default configurations
const axiosInstance = axios.create({
  baseURL: import.meta.env.VITE_API_URL, // Set base URL from environment variable
  timeout: 10000, // Set timeout for requests
  headers: {
    'Content-Type': 'application/json',
  },
});

// Add a request interceptor if needed (e.g., for adding auth tokens)
axiosInstance.interceptors.request.use(
  config => {
    // // Modify the config before sending the request, e.g., add authorization headers
    // const token = localStorage.getItem('token');
    // if (token) {
    //   config.headers['Authorization'] = `Bearer ${token}`;
    // }
    return config;
  },
  error => {
    return Promise.reject(error);
  }
);

// Handle responses globally if needed
axiosInstance.interceptors.response.use(
  response => response,
  error => {
    // Handle global error responses (like 401 or 500)
    if (error.response && error.response.status === 401) {
      // Handle unauthorized responses, e.g., log out the user
      console.log('Unauthorized access - redirect to login');
    }
    return Promise.reject(error);
  }
);

export default axiosInstance;
