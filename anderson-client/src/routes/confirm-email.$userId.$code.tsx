import { AuthLayout } from '@/components/auth/AuthLayout';
import { confirmEmailfn } from '@/server/auth';
import { queryOptions, useSuspenseQuery } from '@tanstack/react-query'
import { createFileRoute, Link } from '@tanstack/react-router'
import { Suspense } from 'react';

const confirmEmailQueryOptions = (data: { userId: string, code: string }) => queryOptions({
  queryKey: ['confirm-email', data.userId, data.code],
  queryFn: async () => {
    return await confirmEmailfn({ data })
  }
});

export const Route = createFileRoute('/confirm-email/$userId/$code')({
  component: ConfirmEmailLayout,
})

function ConfirmEmailLayout() {
  return (
    <AuthLayout
      subtitle="Verifying your email address."
      title="Verify Email"
    >
      <Suspense fallback={<div>Loading...</div>}>
        <ConfirmEmail />
      </Suspense>
    </AuthLayout>)
}

function ConfirmEmail() {
  const params = Route.useParams();
  const confirmEmailQuery = useSuspenseQuery(confirmEmailQueryOptions(params));

  console.log("confirmEmailQuery", confirmEmailQuery);

  if (!confirmEmailQuery.data?.success) {
    return <div>Failed to confirm email</div>;
  }

  return (
    <div className="mx-auto max-w-md space-y-6 text-center">
      <div className="rounded-md bg-green-50 p-4 text-green-700">
        Email confirmed successfully
      </div>
      <Link
        className="font-medium text-gray-900 hover:underline"
        to="/login"
      >
        Go to Sign In
      </Link>
    </div>)
}

