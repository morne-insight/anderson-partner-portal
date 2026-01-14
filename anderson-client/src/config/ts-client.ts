import { client } from "../api/client.gen";

// Global API client configuration
export const configureApiClient = () => {
  // Server-side SSL certificate handling for development
  if (typeof process !== 'undefined' && process.env.NODE_ENV !== 'production') {
    process.env.NODE_TLS_REJECT_UNAUTHORIZED = '0';
  }

  // Configure the client with base URL
  client.setConfig({
    baseUrl: typeof process !== 'undefined' 
      ? process.env.VITE_API_BASE_URL || 'https://localhost:44395'
      : import.meta.env.VITE_API_BASE_URL || 'https://localhost:44395'
  });

  // Add response interceptor for logging (client-side only)
  if (typeof window !== 'undefined') {
    client.interceptors.response.use((response) => {
      if (response.status === 200) {
        console.log(`request to ${response.url} was successful`);
      }
      return response;
    });
  }
};

// Initialize the client configuration immediately
configureApiClient();