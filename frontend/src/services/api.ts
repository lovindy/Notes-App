import axios from "axios";
import { useAuthStore } from "../stores/auth.store";

const api = axios.create({
  baseURL: "https://localhost:7083/api",
  withCredentials: true,
});

// Only if using token-based auth instead of cookies
api.interceptors.request.use(
  (config) => {
    const authStore = useAuthStore();

    // Check if the user is authenticated and retrieve the token from localStorage
    const token = localStorage.getItem("authToken");

    if (token) {
      config.headers["Authorization"] = `Bearer ${token}`; // Add token to headers
    }

    console.log("Request sent:", {
      url: config.url,
      method: config.method,
      data: config.data,
      headers: config.headers,
    });

    return config;
  },
  (error) => {
    console.error("Request error:", error);
    return Promise.reject(error);
  }
);


export default api;
