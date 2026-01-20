import { client } from "../api/client.gen";
import { postApiAccountRefresh } from "../api";
import type { RefreshTokenDto } from "../api";

// Track retry attempts for each request
const requestRetryMap = new Map<string, number>();
const MAX_RETRY_ATTEMPTS = 3;

// Helper function to clear session and redirect to login
const terminateSession = async () => {
  if (typeof window !== "undefined") {
    // Clear any stored tokens
    localStorage.removeItem('refreshToken');
    sessionStorage.clear();
    
    // Redirect to login page
    window.location.href = '/login';
  } else {
    // Server-side session termination
    try {
      const { useAppSession } = await import("../utils/session");
      const session = await useAppSession();
      await session.clear();
    } catch (e) {
      console.error('Failed to clear server session:', e);
    }
  }
};

// Helper function to refresh access token
const refreshAccessToken = async (refreshToken: string): Promise<string | null> => {
  try {
    const response = await postApiAccountRefresh({
      body: { refreshToken } as RefreshTokenDto
    });
    
    if (response.data?.authenticationToken) {
      // Update session with new tokens
      if (typeof window === "undefined") {
        const { useAppSession } = await import("../utils/session");
        const session = await useAppSession();
        await session.update({
          accessToken: response.data.authenticationToken,
          accessTokenExpiresAt: response.data.expiresIn ? Date.now() + (response.data.expiresIn * 1000) : undefined,
          refreshToken: response.data.refreshToken || refreshToken
        });
      }
      
      return response.data.authenticationToken;
    }
  } catch (error) {
    console.error('Failed to refresh token:', error);
  }
  
  return null;
};

// Global API client configuration
export const configureApiClient = () => {
  // Server-side SSL certificate handling for development
  if (typeof process !== "undefined" && process.env.NODE_ENV !== "production") {
    process.env.NODE_TLS_REJECT_UNAUTHORIZED = "0";
  }

  // Configure the client with base URL
  client.setConfig({
    baseUrl:
      typeof process !== "undefined"
        ? process.env.VITE_API_BASE_URL || "https://localhost:44395"
        : import.meta.env.VITE_API_BASE_URL || "https://localhost:44395",
  });

  // Global Request Interceptor for Authentication
  client.interceptors.request.use(async (request) => {
    // Only run this on the server
    if (typeof window === "undefined") {
      try {
        // Dynamic import to avoid bringing server-only code to the browser
        const { useAppSession } = await import("../utils/session");
        const session = await useAppSession();
        const token = session.data.accessToken || session.data.userId;
        if (token) {
          request.headers.set("Authorization", `Bearer ${token}`);
        }
      } catch (e) {
        // Not in a request context (e.g., during build/init)
      }
    }
    return request;
  });
  
  // Add response interceptor for handling authentication errors and retries
  client.interceptors.response.use(async (response, request, options) => {
    // Success response - clear retry count for this request
    const requestKey = `${request.method || 'unknown'}-${response.url}`;
    requestRetryMap.delete(requestKey);
    
    if (typeof window !== "undefined" && response.status === 200) {
      console.log(`request to ${response.url} was successful`);
    }
    return response;
  });
  
  // Add error interceptor for handling authentication errors and retries
  client.interceptors.error.use(async (error: any, response?: Response, request?: Request, options?: any) => {
    console.log('request', request);
    console.log('response', response);
    console.log('error', error);
    if (!request || !response) {
      return error;
    }
    
    const requestKey = `${request.method || 'unknown'}-${request.url || 'unknown'}`;
    const currentRetries = requestRetryMap.get(requestKey) || 0;
    
    // Handle 401 Unauthorized errors
    if (response.status === 401 && currentRetries < MAX_RETRY_ATTEMPTS) {
      requestRetryMap.set(requestKey, currentRetries + 1);
      
      try {
        let refreshToken: string | null = null;
        
        // Get refresh token from appropriate storage
        if (typeof window === "undefined") {
          // Server-side
          const { useAppSession } = await import("../utils/session");
          const session = await useAppSession();
          refreshToken = session.data.refreshToken ?? null;
        } else {
          // Client-side
          refreshToken = localStorage.getItem('refreshToken');
        }
        
        if (refreshToken) {
          const newAccessToken = await refreshAccessToken(refreshToken);
          
          if (newAccessToken) {
            // Create a new request with the updated token
            const newRequest = new Request(request.url, {
              method: request.method,
              headers: {
                ...Object.fromEntries(request.headers.entries()),
                'Authorization': `Bearer ${newAccessToken}`
              },
              body: request.body
            });
            
            // Retry the request with the new token
            try {
              const retryResponse = await fetch(newRequest);
              if (retryResponse.ok) {
                requestRetryMap.delete(requestKey);
                return retryResponse;
              }
            } catch (retryError) {
              console.error('Retry request failed:', retryError);
            }
          }
        }
      } catch (refreshError) {
        console.error('Token refresh failed:', refreshError);
      }
      
      // If we've reached max retries or refresh failed, terminate session
      if (currentRetries >= MAX_RETRY_ATTEMPTS - 1) {
        console.warn(`Max retry attempts (${MAX_RETRY_ATTEMPTS}) reached for ${requestKey}. Terminating session.`);
        requestRetryMap.delete(requestKey);
        await terminateSession();
      }
    } else if (response.status === 401) {
      // Already at max retries, terminate session immediately
      console.warn('Authentication failed after max retries. Terminating session.');
      requestRetryMap.delete(requestKey);
      await terminateSession();
    }
    
    return error;
  });
};

// Initialize the client configuration immediately
configureApiClient();
