// services/httpClient.js
import axiosInstance from "./axiosInstance";

class HttpClient {
  constructor(axiosInstance) {
    this.client = axiosInstance; // Use the passed axiosInstance
  }

  // Method to set the Authorization token
  setToken(token) {
    if (token) {
      this.client.defaults.headers["Authorization"] = `Bearer ${token}`;
    } else {
      delete this.client.defaults.headers["Authorization"]; // Remove token if none is provided
    }
  }

  // Method for GET requests
  get(url, params = {}, config = {}) {
    return this.client.get(url, { params, ...config });
  }

  // Method for POST requests
  post(url, data, config = {}) {
    return this.client.post(url, data, { ...config });
  }

  // Method for PUT requests
  put(url, data, config = {}) {
    return this.client.put(url, data, { ...config });
  }

  // Method for DELETE requests
  delete(url, config = {}) {
    return this.client.delete(url, { ...config });
  }

  // Method for custom HTTP requests
  request(config) {
    return this.client.request(config);
  }
}

// Export an instance of HttpClient with the shared axiosInstance
const httpClient = new HttpClient(axiosInstance);

export default httpClient;
