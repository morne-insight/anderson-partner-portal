import { AuthLayout } from '@/components/auth/AuthLayout'
import { ResendConfirmationForm } from '@/components/auth/ResendConfirmation'
import { createFileRoute } from '@tanstack/react-router'

export const Route = createFileRoute('/resend-email')({
  component: ResendPage,
})

function ResendPage() {
  return (
    <AuthLayout
      title="Resend Account Confirmation"
      subtitle="We'll send you a new confirmation email"
    >
      <ResendConfirmationForm />
    </AuthLayout>
  )
}
