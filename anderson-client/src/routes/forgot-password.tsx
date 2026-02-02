import { AuthLayout } from '@/components/auth/AuthLayout'
import { ForgotPasswordForm } from '@/components/auth/ForgotPasswordForm'
import { createFileRoute } from '@tanstack/react-router'

export const Route = createFileRoute('/forgot-password')({
  component: ForgotPasswordPage,
})

function ForgotPasswordPage() {
  return <AuthLayout title="Forgot Password" subtitle="Enter your email address and we'll send you a link to reset your password.">
    <ForgotPasswordForm />
  </AuthLayout>
}
