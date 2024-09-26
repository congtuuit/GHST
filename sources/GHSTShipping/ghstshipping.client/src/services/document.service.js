// services/auth.service.js
import httpClient from "@/app/httpClient";

class DocumentService {
  // Before making API calls, set the token
  static async getErrorCodes() {
    try {
      const response = await httpClient.get("/api/v1/Doc/GetErrorCodes");
      return response.data;
    } catch (error) {
      console.error("Error fetching users:", error);
      throw error;
    }
  }
}

export default DocumentService;
