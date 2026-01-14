import { createFileRoute, Link } from '@tanstack/react-router'
import { RegisterForm } from '../components/auth/RegisterForm'

export const Route = createFileRoute('/register')({
  component: RegisterPage,
})

function RegisterPage() {
  return (<div>
    <RegisterForm />
    <Link to="/dashboard">Go to Dashboard</Link>
    </div>
    )
}
