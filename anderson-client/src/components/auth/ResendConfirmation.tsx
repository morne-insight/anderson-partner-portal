import { useForm } from "@tanstack/react-form";
import { Link } from "@tanstack/react-router";
import { useServerFn } from "@tanstack/react-start";
import { useState } from "react";
import { z } from "zod";
import { resendConfirmationFn } from "../../server/auth";
import { Button } from "../ui/button";
import { Input } from "../ui/input";
import { Label } from "../ui/label";

const resendSchema = z
    .object({
        email: z.string().email("Invalid email address"),
    });

export function ResendConfirmationForm() {
    const [error, setError] = useState<string | null>(null);
    const [success, setSuccess] = useState(false);
    const [isLoading, setIsLoading] = useState(false);
    const resend = useServerFn(resendConfirmationFn);

    const form = useForm({
        defaultValues: {
            email: "",
        },
        onSubmit: async ({ value }) => {
            setError(null);
            setIsLoading(true);

            try {
                // Validate with Zod before submitting
                const validatedData = resendSchema.parse(value);

                // Call server function
                const result = await resend({
                    data: {
                        email: validatedData.email
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
                    The confirmation email has been resent.
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
                    name="email"
                    validators={{
                        onChange: ({ value }) => {
                            const result = z.email().safeParse(value);
                            return result.success ? undefined : "Email is required";
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

                <Button
                    className="w-full bg-[#DB0A20] text-white hover:bg-[#b0081a]"
                    disabled={isLoading}
                    type="submit"
                >
                    {isLoading ? "Sending..." : "Send Confirmation Email"}
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
