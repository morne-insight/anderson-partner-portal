import { createFileRoute, redirect } from '@tanstack/react-router'
import BackOffice from './back-office'

export const Route = createFileRoute('/_app/admin')({
  beforeLoad: ({ context }) => {
    if (!('user' in context) || !context.user?.roles?.includes('SystemAdmin')) {
      throw redirect({ to: '/dashboard' })
    }
  },
  component: BackOffice,
})
