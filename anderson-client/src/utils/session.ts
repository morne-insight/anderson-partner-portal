// src/utils/session.ts

import { useSession } from "@tanstack/react-start/server";
import type { UserCompanyDto } from "@/api";

export interface SessionData {
  userId?: string;
  userName?: string;
  companyId?: string;
  companyName?: string;
  companies?: Array<UserCompanyDto>;
  email?: string;
  roles?: string[];
  accessToken?: string; // store JWT here (server-only cookie session)
  accessTokenExpiresAt?: number;
  refreshToken?: string; // store refresh token for token renewal
}

export function useAppSession() {
  return useSession<SessionData>({
    name: "app-session",
    password:
      process.env.SESSION_SECRET || "anderson-partner-portal-secret-key",
    cookie: {
      httpOnly: true,
      sameSite: "lax",
      secure: process.env.NODE_ENV === "production",
    },
  });
}
