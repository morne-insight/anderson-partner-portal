import { AuthLayout } from '@/components/auth/AuthLayout';
import { acceptInvitationFn } from '@/server/auth'
import { createFileRoute, Link } from '@tanstack/react-router'

export const Route = createFileRoute('/accept-invite/$inviteId/$userId')({
  component: RouteComponent,
  loader: async ({ params }) => {
    const result = await acceptInvitationFn({
      data: {
        invitationId: params.inviteId,
        userId: params.userId
      }
    });

    return result;
  },
})

function RouteComponent() {

  const showResult = () => {
    const loaderData = Route.useLoaderData();
    if (loaderData.success) {
      return <div className="rounded-md bg-green-50 p-4 text-green-700">
        Invitation accepted successfully!
      </div>
    }
    return <div className="rounded-md bg-red-50 p-4 text-red-700">
      Invitation not accepted!
    </div>
  }

  return <AuthLayout title="Accept Invitation" subtitle="Invitation to join the organization">
    <div className="mx-auto max-w-md space-y-6 text-center">
      {showResult()}
      <Link
        className="font-medium text-gray-900 hover:underline"
        to="/login"
      >
        Go to Sign In
      </Link>
    </div>

  </AuthLayout>
}
