import { QueryClient, QueryClientProvider } from "@tanstack/react-query";

// Custom retry function that skips retries for 401 errors
// since the API client interceptor handles token refresh
const shouldRetryQuery = (failureCount: number, error: unknown): boolean => {
  // Don't retry on 401 - the interceptor handles token refresh
  if (
    error &&
    typeof error === "object" &&
    ("status" in error && (error as { status?: number }).status === 401)
  ) {
    return false;
  }

  // Don't retry if error message indicates auth failure
  if (
    error &&
    typeof error === "object" &&
    "message" in error &&
    typeof (error as { message?: string }).message === "string" &&
    (error as { message: string }).message.includes("401")
  ) {
    return false;
  }

  // Default: retry up to 3 times for other errors
  return failureCount < 3;
};

export function getContext() {
  const queryClient = new QueryClient({
    defaultOptions: {
      queries: {
        retry: shouldRetryQuery,
        // Prevent showing stale data as error during background refetch
        refetchOnWindowFocus: false,
      },
      mutations: {
        retry: shouldRetryQuery,
      },
    },
  });
  return {
    queryClient,
  };
}

export function Provider({
  children,
  queryClient,
}: {
  children: React.ReactNode;
  queryClient: QueryClient;
}) {
  return (
    <QueryClientProvider client={queryClient}>{children}</QueryClientProvider>
  );
}
