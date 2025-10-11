import { jwtDecode } from "jwt-decode";

// check valid token
export const isTokenValid = (token) => {
  if (!token) return false;

  try {
    const decoded  = jwtDecode(token);
    const currentTime = Math.floor(Date.now() / 1000); // Convert to seconds
    return decoded.exp > currentTime; // true if token not expired
  } catch (error) {
    return false; // Invalid token format
  }
};

export const getHeaders = () => ({
    'Content-Type': 'application/json',
    Authorization: `Bearer ${getToken()}`
});

// adjust token / jwt
export const removeToken = () => {
    localStorage.removeItem("jwt");
    window.location.href = "/auth";
}
export const getToken = () => {
    return localStorage.getItem("jwt");
}
export const setToken = (token) => {
    localStorage.setItem("jwt", token);
}
export const isAuthenticated = () => {
    var token = localStorage.getItem("jwt")
    return token != null;
}

// set currentTarget 
export const setUserLog = (user) => {
    localStorage.setItem("user", user);
}

export const getUser = () => {
    return localStorage.getItem("user");
}
export const removeUser = () => {
    return localStorage.removeItem("user");
}