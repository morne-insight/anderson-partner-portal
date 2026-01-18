// src/utils/session.ts
import { useSession } from "@tanstack/react-start/server";

export interface SessionData {
  userId?: string;
  email?: string;
  roles?: string[];
  accessToken?: string; // store JWT here (server-only cookie session)
  accessTokenExpiresAt?: number;
}

export function useAppSession() {

  return useSession<SessionData>({
    name: "app-session",
    password: process.env.SESSION_SECRET || "anderson-partner-portal-secret-key",
    cookie: {
      httpOnly: true,
      sameSite: "lax",
      secure: process.env.NODE_ENV === "production",
    },
  });
}
