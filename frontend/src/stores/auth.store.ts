import { defineStore } from "pinia";
import { ref } from "vue";
import api from "../services/api";
import type { LoginCredentials } from "../interfaces/types";

export const useAuthStore = defineStore("auth", () => {
  const isAuthenticated = ref(false);
  const user = ref<{ username: string } | null>(null);
  const isLoading = ref(true);

  // Register user
  async function register(credentials: LoginCredentials) {
    try {
      console.log("Registering user with credentials:", credentials);
      const response = await api.post("/auth/register", credentials);
      isAuthenticated.value = true;
      user.value = response.data.user;
      console.log("Registration successful:", response.data);
      return response.data;
    } catch (error) {
      console.error(
        "Registration failed:",
        (error as any).response?.data || error
      );
      isAuthenticated.value = false;
      user.value = null;
      throw error;
    }
  }

  // Login user
  async function login(credentials: LoginCredentials) {
    try {
      console.log("Logging in with credentials:", credentials);
      const response = await api.post("/auth/login", credentials);

      // Store token in localStorage
      localStorage.setItem("authToken", response.data.token);

      isAuthenticated.value = true;
      user.value = response.data.user;
      console.log("Login successful:", response.data);

      return response.data;
    } catch (error) {
      console.error("Login failed:", (error as any).response?.data || error);
      isAuthenticated.value = false;
      user.value = null;
      throw error;
    }
  }

  // Logout user
  async function logout() {
    try {
      console.log("Logging out...");
      await api.post("/auth/logout");
      console.log("Logout successful.");
    } catch (error) {
      console.error("Logout failed:", (error as any).response?.data || error);
    } finally {
      isAuthenticated.value = false;
      user.value = null;
    }
  }

  // Check authentication status
  async function checkAuth() {
    if (!isLoading.value) return;

    isLoading.value = true;

    try {
      const token = localStorage.getItem("authToken");

      if (!token) {
        throw new Error("No token found.");
      }

      const response = await api.get("/auth/check", {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });

      if (response.status === 200 && response.data.isAuthenticated) {
        console.log("Authenticated user:", response.data.username);
        isAuthenticated.value = true;
        user.value = { username: response.data.username }; // Update user data
      } else {
        throw new Error("Authentication check failed.");
      }
    } catch (error) {
      console.error("Failed to authenticate:", (error as any).message);
      isAuthenticated.value = false;
      user.value = null;
    } finally {
      isLoading.value = false;
    }
  }

  return {
    isLoading,
    isAuthenticated,
    user,
    login,
    register,
    logout,
    checkAuth,
  };
});
