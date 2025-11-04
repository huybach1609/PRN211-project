import { jwtDecode } from "jwt-decode";

interface DecodedToken {
  exp: number;
  [key: string]: any;
}

// check valid token
export const isTokenValid = (token: string | null): boolean => {
  if (!token) return false;

  try {
    const decoded = jwtDecode<DecodedToken>(token);
    const currentTime = Math.floor(Date.now() / 1000); // Convert to seconds
    return decoded.exp > currentTime; // true if token not expired
  } catch (error) {
    return false; // Invalid token format
  }
};

export const getHeaders = () => ({
  "Content-Type": "application/json",
  Authorization: `Bearer ${getToken()}`,
});

// adjust token / jwt
export const removeToken = () => {
  localStorage.removeItem("jwt");
};
export const getToken = (): string | null => {
  return localStorage.getItem("jwt");
};
export const setToken = (token: string) => {
  localStorage.setItem("jwt", token);
};
export const isAuthenticated = (): boolean => {
  const token = localStorage.getItem("jwt");
  return token != null;
};

// set currentTarget
export const setUserLog = (user: string) => {
  console.log("tokenManage:User ",user);
  localStorage.setItem("user", user);
};

export const getUser = (): string | null => {
  return localStorage.getItem("user");
};
export const removeUser = () => {
  return localStorage.removeItem("user");
};

