import { Link, useParams } from "@tanstack/react-router";
import { useServerFn } from "@tanstack/react-start";
import { confirmEmailfn } from '@/server/auth';
import { useQuery } from "@tanstack/react-query";

export function ConfirmEmail() {
    const params = useParams({ from: "/confirm-email/$userId/$code" });
    const confirmEmail = useServerFn(confirmEmailfn);

    const {
        data,
        isLoading,
        isError,
        error,
    } = useQuery({
        queryKey: ["confirm-email", params.userId, params.code],
        queryFn: () => confirmEmail({ data: { userId: params.userId, code: params.code } }),
    });


    if (isError) {
        return (
            <div className="mx-auto max-w-md space-y-6 text-center">
                <div className="rounded-md bg-red-50 p-4 text-red-700">
                    Email confirmation failed
                    <p>{JSON.stringify(error, null, 2)}</p>
                </div>
                <Link
                    className="font-medium text-gray-900 hover:underline"
                    to="/login"
                >
                    Go to Sign In
                </Link>
            </div>)
    }

    if (isLoading) {
        return (
            <div className="mx-auto max-w-md space-y-6 text-center">
                <div className="rounded-md bg-red-50 p-4 text-red-700">
                    Verifying email...
                </div>
            </div>)
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
