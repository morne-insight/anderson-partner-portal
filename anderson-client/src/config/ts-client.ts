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
    const requestKey = `${request.method || 'unknown'}-${response.url}`;
    
    // Handle 401 Unauthorized errors with token refresh
    if (response.status === 401) {
      const currentRetries = requestRetryMap.get(requestKey) || 0;
      
      if (currentRetries < MAX_RETRY_ATTEMPTS) {
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
            console.log(`Attempting token refresh (attempt ${currentRetries + 1}/${MAX_RETRY_ATTEMPTS})`);
            const newAccessToken = await refreshAccessToken(refreshToken);
            
            if (newAccessToken) {
              // Update the original request with new token
              const newHeaders = new Headers(request.headers);
              newHeaders.set('Authorization', `Bearer ${newAccessToken}`);
              
              // Retry the request with the new token using fetch directly
              try {
                console.log('Retrying request with new token');
                const retryResponse = await fetch(request.url, {
                  method: request.method,
                  headers: newHeaders,
                  body: request.body,
                  mode: request.mode,
                  credentials: request.credentials,
                  cache: request.cache,
                  redirect: request.redirect,
                  referrer: request.referrer,
                  referrerPolicy: request.referrerPolicy,
                  integrity: request.integrity,
                  keepalive: request.keepalive,
                  signal: request.signal
                });
                
                if (retryResponse.ok || retryResponse.status !== 401) {
                  requestRetryMap.delete(requestKey);
                  console.log('Token refresh and retry successful');
                  return retryResponse;
                } else {
                  console.warn('Retry still returned 401, continuing with retry logic');
                }
              } catch (retryError) {
                console.error('Retry request failed:', retryError);
              }
            } else {
              console.warn('Token refresh failed - no new access token received');
            }
          } else {
            console.warn('No refresh token available for token refresh');
          }
        } catch (refreshError) {
          console.error('Token refresh failed:', refreshError);
        }
        
        // If we've reached max retries or refresh failed, terminate session
        if (currentRetries >= MAX_RETRY_ATTEMPTS - 1) {
          console.warn(`Max retry attempts (${MAX_RETRY_ATTEMPTS}) reached for ${requestKey}. Terminating session.`);
          requestRetryMap.delete(requestKey);
          await terminateSession();
          return response; // Return the original 401 response
        }
      } else {
        // Already at max retries, terminate session immediately
        console.warn('Authentication failed after max retries. Terminating session.');
        requestRetryMap.delete(requestKey);
        await terminateSession();
      }
    } else {
      // Success response - clear retry count for this request
      requestRetryMap.delete(requestKey);
      
      if (typeof window !== "undefined" && response.status === 200) {
        console.log(`request to ${response.url} was successful`);
      }
    }
    
    return response;
  });
};

// Initialize the client configuration immediately
configureApiClient();
