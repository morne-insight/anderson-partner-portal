import { useQuery } from "@tanstack/react-query";
import { useRouter } from "@tanstack/react-router";
import { createContext, type ReactNode, useContext } from "react";
import { getCurrentUserFn, logoutFn } from "../server/auth";

type User = {
  id: string;
  email: string;
};

type AuthContextType = {
  user: User | null;
  isLoading: boolean;
  refetch: () => void;
  logout: () => Promise<void>;
};

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export function AuthProvider({ children }: { children: ReactNode }) {
  const router = useRouter();
  const {
    data: user,
    isLoading,
    refetch,
  } = useQuery({
    queryKey: ["currentUser"],
    queryFn: () => getCurrentUserFn(),
    retry: false,
    staleTime: 5 * 60 * 1000, // 5 minutes
  });

  const logout = async () => {
    const result = await logoutFn();
    if (result?.success) {
      // Refetch to clear user state and navigate to home
      await refetch();
      router.navigate({ to: "/" });
    }
  };

  return (
    <AuthContext.Provider
      value={{ user: user || null, isLoading, refetch, logout }}
    >
      {children}
    </AuthContext.Provider>
  );
}

export function useAuth() {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error("useAuth must be used within AuthProvider");
  }
  return context;
}
