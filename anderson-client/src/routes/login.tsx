import { createFileRoute } from "@tanstack/react-router";
import { AuthLayout } from "../components/auth/AuthLayout";
import { LoginForm } from "../components/auth/LoginForm";

export const Route = createFileRoute("/login")({
  component: LoginPage,
});

function LoginPage() {
  return (
    <AuthLayout
      subtitle="Please enter your details to sign in."
      title="Welcome Back"
    >
      <LoginForm />
    </AuthLayout>
  );
}
