import { createFileRoute, Outlet, redirect } from "@tanstack/react-router";
import { getCurrentUserFn } from "../server/auth";
import { MainLayout } from "../components/Layout/MainLayout";

export const Route = createFileRoute("/_app")({
  beforeLoad: async () => {
    const user = await getCurrentUserFn();
    if (!user) {
      throw redirect({ to: "/login" });
    }
    return { user };
  },
  component: () => (
    <MainLayout>
      <Outlet />
    </MainLayout>
  ),
});
