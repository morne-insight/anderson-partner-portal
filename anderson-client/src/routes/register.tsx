import { createFileRoute } from "@tanstack/react-router";
import { AuthLayout } from "../components/auth/AuthLayout";
import { RegisterForm } from "../components/auth/RegisterForm";

export const Route = createFileRoute("/register")({
  component: RegisterPage,
});

function RegisterPage() {
  return (
    <AuthLayout
      title="Create an Account"
      subtitle="Join Anderson to start your journey."
    >
      <RegisterForm />
    </AuthLayout>
  );
}
