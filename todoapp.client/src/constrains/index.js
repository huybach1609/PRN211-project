// Check if VITE_API_URL environment variable is set from runtime injection
const runtimeApiUrl = window.__ENV__?.VITE_API_URL;
const buildTimeApiUrl = import.meta.env.VITE_API_URL;

// Use runtime value first, fallback to build-time value
const apiUrl = runtimeApiUrl || buildTimeApiUrl;

// if (!apiUrl) {
//   // Redirect to error page if VITE_API_URL is not found
//   window.location.href = '/error/configuration';
// }

export const API_URL = "/api";
