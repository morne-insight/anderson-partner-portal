import { AuthLayout } from '@/components/auth/AuthLayout';
import { ConfirmEmail } from '@/components/auth/ConfirmEmail';
import { createFileRoute } from '@tanstack/react-router'


export const Route = createFileRoute('/confirm-email/$userId/$code')({
  component: ConfirmEmailPage,
})

function ConfirmEmailPage() {
  return (
    <AuthLayout
      subtitle="Verifying your email address."
      title="Verify Email"
    >
      <ConfirmEmail />
    </AuthLayout>)
}

