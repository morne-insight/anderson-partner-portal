import { createFileRoute, Outlet, redirect } from "@tanstack/react-router";
import { getCurrentUserFn } from "../server/auth";

export const Route = createFileRoute("/_app")({
  beforeLoad: async () => {
    const user = await getCurrentUserFn();
    console.log("user", user);
    if (!user) {
      throw redirect({ to: "/login" });
    }
    return { user };
  },
  component: () => <Outlet />,
});
