import { useForm } from "@tanstack/react-form";
import { Link, useParams } from "@tanstack/react-router";
import { useServerFn } from "@tanstack/react-start";
import { useState } from "react";
import { z } from "zod";
import { resetPasswordFn } from "../../server/auth";
import { Button } from "../ui/button";
import { Input } from "../ui/input";
import { Label } from "../ui/label";

const resetPasswordSchema = z
  .object({
    email: z.string().email("Invalid email address"),
    newPassword: z.string().min(6, "Password must be at least 6 characters"),
    resetCode: z.string(),
  });

export function ResetPasswordForm() {
  const params = useParams({ from: "/reset-password/$email/$resetCode" });
  const [error, setError] = useState<string | null>(null);
  const [success, setSuccess] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  const resetPassword = useServerFn(resetPasswordFn);

  const form = useForm({
    defaultValues: {
      email: params.email,
      resetCode: params.resetCode,
      newPassword: "",
    },
    onSubmit: async ({ value }) => {
      setError(null);
      setIsLoading(true);

      try {
        // Validate with Zod before submitting
        const validatedData = resetPasswordSchema.parse(value);

        // Call server function
        const result = await resetPassword({
          data: {
            resetCode: validatedData.resetCode,
            email: validatedData.email,
            newPassword: validatedData.newPassword,
          },
        });

        if (result?.error) {
          setError(result.error);
        } else {
          setSuccess(true);
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

  if (success) {
    return (
      <div className="mx-auto max-w-md space-y-6 text-center">
        <div className="rounded-md bg-green-50 p-4 text-green-700">
          Password reset successful! You can sign in with your credentials.
        </div>
        <Link
          className="font-medium text-[#DB0A20] hover:underline"
          to="/login"
        >
          Go to Sign In
        </Link>
      </div>
    );
  }

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
          name="newPassword"
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
              <Label htmlFor="password">New Password</Label>
              <Input
                id="password"
                onBlur={field.handleBlur}
                onChange={(e) => field.handleChange(e.target.value)}
                placeholder="Enter your new password"
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
          {isLoading ? "Resetting Password..." : "Reset Password"}
        </Button>
      </form>

      <p className="text-center text-sm">
        Already have an account?{" "}
        <Link
          className="font-medium text-[#DB0A20] hover:underline"
          to="/login"
        >
          Sign in
        </Link>
      </p>
    </div>
  );
}
