import { useForm } from '@tanstack/react-form'
import { z } from 'zod'
import { useState } from 'react'
import { Link } from '@tanstack/react-router'
import { registerFn } from '../../server/auth'
import { Button } from '../ui/button'
import { Input } from '../ui/input'
import { Label } from '../ui/label'
import { useServerFn } from '@tanstack/react-start'

const registerSchema = z.object({
  email: z.string().email('Invalid email address'),
  password: z.string().min(6, 'Password must be at least 6 characters'),
  confirmPassword: z.string()
}).refine((data) => data.password === data.confirmPassword, {
  message: "Passwords don't match",
  path: ["confirmPassword"]
})

export function RegisterForm() {
  const [error, setError] = useState<string | null>(null)
  const [success, setSuccess] = useState(false)
  const [isLoading, setIsLoading] = useState(false)
  const register = useServerFn(registerFn)

  const form = useForm({
    defaultValues: {
      email: 'mvanzyl@insight.com',
      password: 'Password$123',
      confirmPassword: 'Password$123'
    },
    onSubmit: async ({ value }) => {
      setError(null)
      setIsLoading(true)
      
      try {
        // Validate with Zod before submitting
        const validatedData = registerSchema.parse(value)
        
        // Call server function
        const result = await register({ 
          data: { 
            email: validatedData.email, 
            password: validatedData.password 
          } 
        })
        
        if (result?.error) {
          setError(result.error)
        } else {
          setSuccess(true)
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

  if (success) {
    return (
      <div className="mx-auto max-w-md space-y-6 text-center">
        <div className="rounded-md bg-green-50 p-4 text-green-700">
          Registration successful! You can now sign in with your credentials.
        </div>
        <Link to="/login" className="text-blue-600 hover:underline">
          Go to Sign In
        </Link>
      </div>
    )
  }

  return (
    <div className="mx-auto max-w-md space-y-6">
      <div className="space-y-2 text-center">
        <h1 className="text-3xl font-bold">Sign Up</h1>
        <p className="text-gray-500">Create a new account to get started</p>
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

        <form.Field 
          name="confirmPassword"
          validators={{
            onChange: ({ value }) => {
              const password = form.getFieldValue('password')
              return value === password ? undefined : "Passwords don't match"
            }
          }}
        >
          {(field) => (
            <div className="space-y-2">
              <Label htmlFor="confirmPassword">Confirm Password</Label>
              <Input
                id="confirmPassword"
                type="password"
                value={field.state.value}
                onBlur={field.handleBlur}
                onChange={(e) => field.handleChange(e.target.value)}
                placeholder="Confirm your password"
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
          {isLoading ? 'Creating Account...' : 'Sign Up'}
        </Button>
      </form>

      <p className="text-center text-sm">
        Already have an account?{' '}
        <Link to="/login" className="text-blue-600 hover:underline">
          Sign in
        </Link>
      </p>
    </div>
  )
}
