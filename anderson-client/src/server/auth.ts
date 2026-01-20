import { createServerFn } from "@tanstack/react-start";
import {
  postApiAccountLogin,
  postApiAccountLogout,
  postApiAccountRegister,
} from "../api";
import { useAppSession } from "../utils/session";

// Login server function
export const loginFn = createServerFn({ method: "POST" })
  .inputValidator((data: { email: string; password: string }) => data)
  .handler(async ({ data }) => {
    try {
      const response = await postApiAccountLogin({
        body: data,
      });

      if (!response.data) {
        return { error: "Invalid credentials" };
      }

      // Create session with user data and tokens
      const session = await useAppSession();
      await session.update({
        userId: response.data.authenticationToken ?? undefined, // or extract user ID from JWT
        email: data.email,
        accessToken: response.data.authenticationToken ?? undefined,
        accessTokenExpiresAt: response.data.expiresIn ? Date.now() + (response.data.expiresIn * 1000) : undefined,
        refreshToken: response.data.refreshToken ?? undefined,
      });

      // Return success instead of throwing redirect
      return { success: true };
    } catch (error) {
      return { error: "Invalid credentials" };
    }
  });

// Register server function
export const registerFn = createServerFn({ method: "POST" })
  .inputValidator((data: { email: string; password: string }) => data)
  .handler(async ({ data }) => {
    try {
      await postApiAccountRegister({
        body: data,
      });

      return { success: true };
    } catch (error) {
      console.log(String(error));
      return { error: "Registration failed" };
    }
  });

// Logout server function
export const logoutFn = createServerFn({ method: "POST" }).handler(async () => {
  const session = await useAppSession();

  // Call API logout if needed
  try {
    await postApiAccountLogout();
  } catch (error) {
    // Continue with local logout even if API call fails
  }

  await session.clear();
  return { success: true };
});

// Get current user
export const getCurrentUserFn = createServerFn({ method: "GET" }).handler(
  async () => {
    const session = await useAppSession();
    const userId = session.data.userId;

    if (!userId) {
      return null;
    }

    // Return user data from session or fetch from API
    return {
      id: userId,
      email: session.data.email!,
    };
  }
);
