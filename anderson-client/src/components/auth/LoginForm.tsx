import { useForm } from "@tanstack/react-form";
import { Link, useRouter } from "@tanstack/react-router";
import { useState } from "react";
import { z } from "zod";
import { useAuth } from "../../contexts/auth-context";
import { loginFn } from "../../server/auth";
import { Button } from "../ui/button";
import { Input } from "../ui/input";
import { Label } from "../ui/label";

const loginSchema = z.object({
  email: z.string().email("Invalid email address"),
  password: z.string().min(6, "Password must be at least 6 characters"),
});

export function LoginForm() {
  const [error, setError] = useState<string | null>(null);
  const [isLoading, setIsLoading] = useState(false);
  const router = useRouter();
  const { refetch } = useAuth();

  const form = useForm({
    defaultValues: {
      email: "mvanzyl@insight.com",
      password: "Password$123",
    },
    onSubmit: async ({ value }) => {
      setError(null);
      setIsLoading(true);

      try {
        // Validate with Zod before submitting
        const validatedData = loginSchema.parse(value);

        // Call server function
        const result = await loginFn({ data: validatedData });

        // Handle the response
        if (result?.error) {
          setError(result.error);
        } else if (result?.success) {
          // Refetch auth context to update Header
          await refetch();
          // Navigate to dashboard on successful login
          router.navigate({ to: "/dashboard" });
        }
      } catch (validationError) {
        if (validationError instanceof z.ZodError) {
          setError(validationError.issues[0]?.message || "Validation error");
        } else {
          setError("An unexpected error occurred. Please try again.");
        }
      } finally {
        setIsLoading(false);
      }
    },
  });

  return (
    <div className="mx-auto max-w-md space-y-6">
      {/* Header removed as it is now handled by AuthLayout */}

      {error && (
        <div className="rounded-md bg-red-50 p-4 text-red-700">{error}</div>
      )}

      <form
        className="space-y-4"
        onSubmit={(e) => {
          e.preventDefault();
          e.stopPropagation();
          form.handleSubmit();
        }}
      >
        <form.Field
          name="email"
          validators={{
            onChange: ({ value }) => {
              const result = z.string().email().safeParse(value);
              return result.success ? undefined : "Invalid email address";
            },
          }}
        >
          {(field) => (
            <div className="space-y-2">
              <Label htmlFor="email">Email</Label>
              <Input
                id="email"
                onBlur={field.handleBlur}
                onChange={(e) => field.handleChange(e.target.value)}
                placeholder="Enter your email"
                type="email"
                value={field.state.value}
              />
              {field.state.meta.errors &&
                field.state.meta.errors.length > 0 && (
                  <p className="text-red-600 text-sm">
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
              const result = z.string().min(6).safeParse(value);
              return result.success
                ? undefined
                : "Password must be at least 6 characters";
            },
          }}
        >
          {(field) => (
            <div className="space-y-2">
              <Label htmlFor="password">Password</Label>
              <Input
                id="password"
                onBlur={field.handleBlur}
                onChange={(e) => field.handleChange(e.target.value)}
                placeholder="Enter your password"
                type="password"
                value={field.state.value}
              />
              {field.state.meta.errors &&
                field.state.meta.errors.length > 0 && (
                  <p className="text-red-600 text-sm">
                    {field.state.meta.errors[0]}
                  </p>
                )}
            </div>
          )}
        </form.Field>

        <Button
          className="w-full bg-[#DB0A20] text-white hover:bg-[#b0081a]"
          disabled={isLoading}
          type="submit"
        >
          {isLoading ? "Signing In..." : "Sign In"}
        </Button>
      </form>

      <p className="text-center text-sm">
        Don't have an account?{" "}
        <Link
          className="font-medium text-[#DB0A20] hover:underline"
          to="/register"
        >
          Sign up
        </Link>
      </p>
    </div>
  );
}
