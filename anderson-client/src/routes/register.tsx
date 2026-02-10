import { createFileRoute } from "@tanstack/react-router";
import { AuthLayout } from "../components/auth/AuthLayout";
import { RegisterForm } from "../components/auth/RegisterForm";

export const Route = createFileRoute("/register")({
  component: RegisterPage,
});

function RegisterPage() {
  return (
    <AuthLayout
      subtitle=""
      title="Create an Account"
    >
      <RegisterForm />
    </AuthLayout>
  );
}
