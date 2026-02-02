import { createFileRoute } from '@tanstack/react-router'
import { AuthLayout } from '@/components/auth/AuthLayout'
import { ResetPasswordForm } from '@/components/auth/ResetPasswordForm'

export const Route = createFileRoute('/reset-password/$email/$resetCode')({
  component: ResetPasswordPage,
})

function ResetPasswordPage() {
  return <AuthLayout subtitle="Reset your password" title="Reset Password">
    <ResetPasswordForm />
  </AuthLayout>
}
