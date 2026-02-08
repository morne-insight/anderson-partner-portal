import { useMutation, useQueryClient } from '@tanstack/react-query'
import { useRouter } from '@tanstack/react-router'
import { Building2, Check, Globe, Loader2, X } from 'lucide-react'
import { useState } from 'react'
import { toast } from 'sonner'
import type { InviteDto } from '@/api'
import {
    AlertDialog,
    AlertDialogAction,
    AlertDialogCancel,
    AlertDialogContent,
    AlertDialogDescription,
    AlertDialogFooter,
    AlertDialogHeader,
    AlertDialogTitle,
} from '@/components/ui/alert-dialog'
import { Button } from '@/components/ui/button'
import { useAuth } from '@/contexts/auth-context'
import { callApi } from '@/server/proxy'

interface InviteListProps {
    invites: InviteDto[]
}

export function InviteList({ invites }: InviteListProps) {
    const router = useRouter()
    const queryClient = useQueryClient()
    const { user } = useAuth()
    const [declineDialogOpen, setDeclineDialogOpen] = useState(false)
    const [selectedInvite, setSelectedInvite] = useState<InviteDto | null>(null)

    const acceptMutation = useMutation({
        mutationFn: async (invite: InviteDto) => {
            return await callApi({
                data: {
                    fn: 'putApiInvitesAccept',
                    args: {
                        body: {
                            id: invite.id,
                            userId: null,
                        },
                    },
                },
            })
        },
        onSuccess: () => {
            toast.success('Invitation accepted! You now have access to the company.')
            queryClient.invalidateQueries({ queryKey: ['invites'] })
            router.invalidate()
        },
        onError: (err) => {
            console.error('Accept failed', err)
            toast.error('Failed to accept invitation. Please try again.')
        },
    })

    const declineMutation = useMutation({
        mutationFn: async (inviteId: string) => {
            return await callApi({
                data: {
                    fn: 'deleteApiInvitesById',
                    args: {
                        path: { id: inviteId },
                    },
                },
            })
        },
        onSuccess: () => {
            toast.success('Invitation declined.')
            queryClient.invalidateQueries({ queryKey: ['invites'] })
            router.invalidate()
            setDeclineDialogOpen(false)
            setSelectedInvite(null)
        },
        onError: (err) => {
            console.error('Decline failed', err)
            toast.error('Failed to decline invitation. Please try again.')
        },
    })

    const handleDeclineClick = (invite: InviteDto) => {
        setSelectedInvite(invite)
        setDeclineDialogOpen(true)
    }

    const handleConfirmDecline = () => {
        if (selectedInvite?.id) {
            declineMutation.mutate(selectedInvite.id)
        }
    }

    if (!invites || invites.length === 0) {
        return null
    }

    return (
        <>
            <div className="mt-4 grid grid-cols-1 gap-4 md:grid-cols-2 lg:grid-cols-3">
                {invites.map((invite) => (
                    <div
                        className="group relative overflow-hidden rounded-lg border border-gray-200 bg-white transition-all duration-300 hover:border-red-600/30 hover:shadow-lg"
                        key={invite.id}
                    >
                        {/* Accent bar */}
                        <div className="absolute top-0 left-0 h-1 w-full bg-gradient-to-r from-red-600 to-red-400" />

                        <div className="p-6">
                            {/* Company info */}
                            <div className="mb-4">
                                <div className="mb-2 flex items-start gap-3">
                                    <div className="flex h-10 w-10 shrink-0 items-center justify-center rounded-full bg-gray-100 transition-colors group-hover:bg-red-50">
                                        <Building2 className="h-5 w-5 text-gray-600 group-hover:text-red-600" />
                                    </div>
                                    <div className="min-w-0 flex-1">
                                        <h4 className="truncate font-serif text-lg text-black">
                                            {invite.companyName || 'Unknown Company'}
                                        </h4>
                                        {invite.companyWebsiteUrl && (
                                            <a
                                                className="flex items-center gap-1 text-gray-400 text-xs transition-colors hover:text-red-600"
                                                href={
                                                    invite.companyWebsiteUrl.startsWith('http')
                                                        ? invite.companyWebsiteUrl
                                                        : `https://${invite.companyWebsiteUrl}`
                                                }
                                                rel="noopener noreferrer"
                                                target="_blank"
                                            >
                                                <Globe className="h-3 w-3" />
                                                <span className="truncate">
                                                    {invite.companyWebsiteUrl.replace(/^https?:\/\//, '')}
                                                </span>
                                            </a>
                                        )}
                                    </div>
                                </div>

                                {invite.companyShortDescription && (
                                    <p className="mt-3 line-clamp-2 text-gray-500 text-sm leading-relaxed">
                                        {invite.companyShortDescription}
                                    </p>
                                )}
                            </div>

                            {/* Invited by info */}
                            <div className="mb-4 border-gray-100 border-t pt-4">
                                <p className="text-gray-400 text-xs uppercase tracking-wider">
                                    Invited by
                                </p>
                                <p className="mt-1 font-medium text-gray-700 text-sm">
                                    {invite.name || invite.email || 'Unknown'}
                                </p>
                            </div>

                            {/* Action buttons */}
                            <div className="flex gap-2">
                                <Button
                                    className="flex-1 gap-2 bg-red-600 font-bold text-xs uppercase tracking-wider hover:bg-red-700"
                                    disabled={acceptMutation.isPending}
                                    onClick={() => acceptMutation.mutate(invite)}
                                >
                                    {acceptMutation.isPending ? (
                                        <Loader2 className="h-4 w-4 animate-spin" />
                                    ) : (
                                        <Check className="h-4 w-4" />
                                    )}
                                    Accept
                                </Button>
                                <Button
                                    className="flex-1 gap-2 border-gray-300 font-bold text-gray-600 text-xs uppercase tracking-wider hover:border-red-600 hover:bg-red-50 hover:text-red-600"
                                    disabled={declineMutation.isPending}
                                    onClick={() => handleDeclineClick(invite)}
                                    variant="outline"
                                >
                                    {declineMutation.isPending && selectedInvite?.id === invite.id ? (
                                        <Loader2 className="h-4 w-4 animate-spin" />
                                    ) : (
                                        <X className="h-4 w-4" />
                                    )}
                                    Decline
                                </Button>
                            </div>
                        </div>
                    </div>
                ))}
            </div>

            {/* Decline confirmation dialog */}
            <AlertDialog onOpenChange={setDeclineDialogOpen} open={declineDialogOpen}>
                <AlertDialogContent>
                    <AlertDialogHeader>
                        <AlertDialogTitle>Decline Invitation?</AlertDialogTitle>
                        <AlertDialogDescription className="space-y-2">
                            <span className="block">
                                Are you sure you want to decline the invitation to join{' '}
                                <strong className="text-gray-900">
                                    {selectedInvite?.companyName || 'this company'}
                                </strong>
                                ?
                            </span>
                            <span className="block font-medium text-amber-600">
                                Warning: If you decline, you will need to be re-invited before you can
                                gain access to this company.
                            </span>
                        </AlertDialogDescription>
                    </AlertDialogHeader>
                    <AlertDialogFooter>
                        <AlertDialogCancel disabled={declineMutation.isPending}>
                            Cancel
                        </AlertDialogCancel>
                        <AlertDialogAction
                            disabled={declineMutation.isPending}
                            onClick={handleConfirmDecline}
                            variant="destructive"
                        >
                            {declineMutation.isPending ? (
                                <>
                                    <Loader2 className="mr-2 h-4 w-4 animate-spin" />
                                    Declining...
                                </>
                            ) : (
                                'Yes, Decline Invitation'
                            )}
                        </AlertDialogAction>
                    </AlertDialogFooter>
                </AlertDialogContent>
            </AlertDialog>
        </>
    )
}
