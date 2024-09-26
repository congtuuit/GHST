// services/auth.service.js
import httpClient from "@/app/httpClient";

class ApiService {
  // Before making API calls, set the token
  static async initializeToken() {
    const token = localStorage.getItem('token');
    httpClient.setToken(token); // Set token in the client
  }

  static async getUsers() {
    try {
      await this.initializeToken(); // Ensure token is set
      const response = await httpClient.get('/users');
      return response.data;
    } catch (error) {
      console.error('Error fetching users:', error);
      throw error;
    }
  }

  static async createUser(userData) {
    try {
      await this.initializeToken(); // Ensure token is set
      const response = await httpClient.post('/users', userData);
      return response.data;
    } catch (error) {
      console.error('Error creating user:', error);
      throw error;
    }
  }
}

export default ApiService;
