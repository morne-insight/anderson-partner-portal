import { useForm } from '@tanstack/react-form'
import { z } from 'zod'
import { useState } from 'react'
import { Link, useRouter } from '@tanstack/react-router'
import { loginFn } from '../../server/auth'
import { useAuth } from '../../contexts/auth-context'
import { Button } from '../ui/button'
import { Input } from '../ui/input'
import { Label } from '../ui/label'

const loginSchema = z.object({
  email: z.string().email('Invalid email address'),
  password: z.string().min(6, 'Password must be at least 6 characters')
})

export function LoginForm() {
  const [error, setError] = useState<string | null>(null)
  const [isLoading, setIsLoading] = useState(false)
  const router = useRouter()
  const { refetch } = useAuth()

  const form = useForm({
    defaultValues: {
      email: 'mvanzyl@insight.com',
      password: 'Password$123',
    },
    onSubmit: async ({ value }) => {
      setError(null)
      setIsLoading(true)
      
      try {
        // Validate with Zod before submitting
        const validatedData = loginSchema.parse(value)
        
        // Call server function
        const result = await loginFn({ data: validatedData })
        
        // Handle the response
        if (result?.error) {
          setError(result.error)
        } else if (result?.success) {
          // Refetch auth context to update Header
          await refetch()
          // Navigate to dashboard on successful login
          router.navigate({ to: '/dashboard' })
        }
      } catch (validationError) {
        if (validationError instanceof z.ZodError) {
          setError(validationError.issues[0]?.message || 'Validation error')
        } else {
          setError('An unexpected error occurred. Please try again.')
        }
      } finally {
        setIsLoading(false)
      }
    }
  })

  return (
    <div className="mx-auto max-w-md space-y-6">
      <div className="space-y-2 text-center">
        <h1 className="text-3xl font-bold">Sign In</h1>
        <p className="text-gray-500">Enter your credentials to access your account</p>
      </div>
      
      {error && (
        <div className="rounded-md bg-red-50 p-4 text-red-700">
          {error}
        </div>
      )}

      <form
        onSubmit={(e) => {
          e.preventDefault()
          e.stopPropagation()
          form.handleSubmit()
        }}
        className="space-y-4"
      >
        <form.Field 
          name="email"
          validators={{
            onChange: ({ value }) => {
              const result = z.string().email().safeParse(value)
              return result.success ? undefined : 'Invalid email address'
            }
          }}
        >
          {(field) => (
            <div className="space-y-2">
              <Label htmlFor="email">Email</Label>
              <Input
                id="email"
                type="email"
                value={field.state.value}
                onBlur={field.handleBlur}
                onChange={(e) => field.handleChange(e.target.value)}
                placeholder="Enter your email"
              />
              {field.state.meta.errors && field.state.meta.errors.length > 0 && (
                <p className="text-sm text-red-600">
                  {field.state.meta.errors[0]}
                </p>
              )}
            </div>
          )}
        </form.Field>

        <form.Field 
          name="password"
          validators={{
            onChange: ({ value }) => {
              const result = z.string().min(6).safeParse(value)
              return result.success ? undefined : 'Password must be at least 6 characters'
            }
          }}
        >
          {(field) => (
            <div className="space-y-2">
              <Label htmlFor="password">Password</Label>
              <Input
                id="password"
                type="password"
                value={field.state.value}
                onBlur={field.handleBlur}
                onChange={(e) => field.handleChange(e.target.value)}
                placeholder="Enter your password"
              />
              {field.state.meta.errors && field.state.meta.errors.length > 0 && (
                <p className="text-sm text-red-600">
                  {field.state.meta.errors[0]}
                </p>
              )}
            </div>
          )}
        </form.Field>

        <Button type="submit" disabled={isLoading} className="w-full">
          {isLoading ? 'Signing In...' : 'Sign In'}
        </Button>
      </form>

      <p className="text-center text-sm">
        Don't have an account?{' '}
        <Link to="/register" className="text-blue-600 hover:underline">
          Sign up
        </Link>
      </p>
    </div>
  )
}
