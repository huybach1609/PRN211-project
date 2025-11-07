import axios, { AxiosRequestConfig } from "axios";
import { API_URL } from "../constrains";
import { removeToken, removeUser } from "./tokenManage";

// Create axios instance with base configuration
export const createApiInstance = (customBaseUrl?: string) => {
  const api = axios.create({
    baseURL: customBaseUrl || API_URL,
    headers: {
      "Content-Type": "application/json",
    },
  });

  // Add token to requests
  api.interceptors.request.use((config) => {
    const token = localStorage.getItem("jwt");
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  });

  // Handle 401 Unauthorized globally
  let isHandlingUnauthorized = false;
  api.interceptors.response.use(
    (response) => response,
    (error) => {
      const status = error?.response?.status;
      if (status === 401 && !isHandlingUnauthorized) {
        isHandlingUnauthorized = true;
        try {
          removeToken();
        } finally {
          // Redirect to auth page; adjust path if your route differs
          if (typeof window !== "undefined") {
            const currentPath = window.location.pathname + window.location.search;
            // avoid redirect loop if already on auth page
            if (!/\/auth(\/|$)/.test(window.location.pathname)) {
              const redirectParam = encodeURIComponent(currentPath);
              window.location.href = `/auth?redirect=${redirectParam}`;
            }
          }
        }
      }
      return Promise.reject(error);
    }
  );

  return api;
};

// Get auth headers for direct axios calls
export const getAuthHeaders = (): Record<string, string> => {
  const token = localStorage.getItem("jwt");
  return {
    Authorization: `Bearer ${token}`,
    "Content-Type": "application/json",
  };
};

// Config for direct axios calls
export const getAuthConfig = (): AxiosRequestConfig => {
  return {
    headers: getAuthHeaders(),
  };
};
