import { useState } from "react";
import { toast } from "sonner";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { Button } from "@/components/ui/button";
import { Badge } from "@/components/ui/badge";
import { Textarea } from "@/components/ui/textarea";
import { MessageCircle, Send, Edit, Trash2, Loader2 } from "lucide-react";
import { callApi } from "@/server/proxy";
import { OpportunityMessageDto, AddMessageOpportunityCommand, UpdateMessageOpportunityCommand } from "@/api";
import { useAuth } from "@/contexts/auth-context";
import {
    AlertDialog,
    AlertDialogAction,
    AlertDialogCancel,
    AlertDialogContent,
    AlertDialogDescription,
    AlertDialogFooter,
    AlertDialogHeader,
    AlertDialogTitle,
    AlertDialogTrigger,
} from "@/components/ui/alert-dialog";

interface OpportunityMessagesProps {
    opportunityId: string;
    canAddMessage?: boolean;
    isOwnOpportunity?: boolean;
}

export function OpportunityMessages({
    opportunityId,
    canAddMessage = true,
    isOwnOpportunity = false
}: OpportunityMessagesProps) {
    const { user } = useAuth();
    const [newMessage, setNewMessage] = useState("");
    const [editingMessage, setEditingMessage] = useState<string | null>(null);
    const [editContent, setEditContent] = useState("");
    const queryClient = useQueryClient();

    // Fetch messages using the new endpoint
    const { data: messages = [], isLoading, isError } = useQuery({
        queryKey: ['opportunity-messages', opportunityId],
        queryFn: () => callApi({
            data: {
                fn: 'getApiOpportunitiesByIdMessages',
                args: {
                    path: { id: opportunityId }
                }
            }
        }),
        staleTime: 30000, // Consider data fresh for 30 seconds
        refetchOnWindowFocus: true
    });

    const addMessageMutation = useMutation({
        mutationFn: async (content: string) => {
            const command: AddMessageOpportunityCommand = {
                opportunityId,
                content,
                createdDate: new Date(),
                createdByUserId: user?.userId!,
                createdByUser: user?.userName || "Anonymous User",
                createdByPartner: user?.companyName || "",
                isOwnMessage: isOwnOpportunity
            };

            return callApi({
                data: { fn: 'postApiOpportunitiesMessage', args: { body: command } }
            });
        },
        onSuccess: () => {
            setNewMessage("");
            // Invalidate the messages query to refetch
            queryClient.invalidateQueries({ queryKey: ['opportunity-messages', opportunityId] });
        },
        onError: (error) => {
            console.error('Failed to add message:', error);
            toast.error('Failed to send message. Please try again.');
        }
    });

    const updateMessageMutation = useMutation({
        mutationFn: async ({ messageId, content }: { messageId: string; content: string }) => {
            const command: UpdateMessageOpportunityCommand = {
                id: opportunityId,
                messageId,
                content
            };
            return await callApi({
                data: { fn: 'putApiOpportunitiesByIdMessage', args: { path: { id: opportunityId }, body: command } }
            });
        },
        onSuccess: () => {
            setEditingMessage(null);
            setEditContent("");
            queryClient.invalidateQueries({ queryKey: ['opportunity-messages', opportunityId] });
        },
        onError: (error) => {
            console.error('Failed to update message:', error);
            toast.error('Failed to update message. Please try again.');
        }
    });

    const deleteMessageMutation = useMutation({
        mutationFn: async (messageId: string) => {
            return callApi({
                data: {
                    fn: 'deleteApiOpportunitiesByIdMessage',
                    args: {
                        path: { id: opportunityId },
                        query: { messageId }
                    }
                }
            });
        },
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['opportunity-messages', opportunityId] });
        },
        onError: (error) => {
            console.error('Failed to delete message:', error);
            toast.error('Failed to delete message. Please try again.');
        }
    });

    const handleAddMessage = () => {
        if (newMessage.trim()) {
            addMessageMutation.mutate(newMessage.trim());
        }
    };

    const handleEditMessage = (messageId: string, content: string) => {
        setEditingMessage(messageId);
        setEditContent(content);
    };

    const handleUpdateMessage = () => {
        if (editingMessage && editContent.trim()) {
            updateMessageMutation.mutate({
                messageId: editingMessage,
                content: editContent.trim()
            });
        }
    };



    const formatTimeAgo = (date: Date | string | undefined) => {
        if (!date) return '';
        const messageDate = new Date(date);
        const now = new Date();
        const diffInHours = Math.floor((now.getTime() - messageDate.getTime()) / (1000 * 60 * 60));

        if (diffInHours < 1) return 'JUST NOW';
        if (diffInHours === 1) return '1 HOUR AGO';
        if (diffInHours < 24) return `${diffInHours} HOURS AGO`;

        const diffInDays = Math.floor(diffInHours / 24);
        if (diffInDays === 1) return '1 DAY AGO';
        return `${diffInDays} DAYS AGO`;
    };

    const sortedMessages = [...messages].sort((a, b) => {
        const dateA = new Date(a.createdDate || 0);
        const dateB = new Date(b.createdDate || 0);
        return dateA.getTime() - dateB.getTime(); // Most recent first
    });

    function canEdit(message: OpportunityMessageDto) {
        const lastMessage = sortedMessages[sortedMessages.length - 1];
        if (message.id !== lastMessage?.id) return false;

        if (message.isOwnMessage) {
            return true;
        }

        return message.createdByUserId === user?.userId;
    }

    // Show loading state
    if (isLoading) {
        return (
            <div className="bg-white border border-gray-200 rounded-none">
                <div className="p-6">
                    <div className="flex items-center gap-2 mb-6">
                        <MessageCircle className="w-5 h-5 text-red-500" />
                        <h3 className="text-lg font-semibold text-gray-900">Clarification Q&A</h3>
                    </div>
                    <div className="flex justify-center items-center py-8">
                        <Loader2 className="w-6 h-6 animate-spin text-gray-400" />
                        <span className="ml-2 text-sm text-gray-500">Loading messages...</span>
                    </div>
                </div>
            </div>
        );
    }

    // Show error state
    if (isError) {
        return (
            <div className="bg-white border border-gray-200 rounded-none">
                <div className="p-6">
                    <div className="flex items-center gap-2 mb-6">
                        <MessageCircle className="w-5 h-5 text-red-500" />
                        <h3 className="text-lg font-semibold text-gray-900">Clarification Q&A</h3>
                    </div>
                    <div className="text-center py-8 text-red-500">
                        <p className="text-sm">Failed to load messages. Please try again.</p>
                        <Button
                            variant="outline"
                            size="sm"
                            onClick={() => queryClient.invalidateQueries({ queryKey: ['opportunity-messages', opportunityId] })}
                            className="mt-2"
                        >
                            Retry
                        </Button>
                    </div>
                </div>
            </div>
        );
    }

    return (
        <div className="bg-white border border-gray-200 rounded-none">
            <div className="p-6">
                <div className="flex items-center gap-2 mb-6">
                    <MessageCircle className="w-5 h-5 text-red-500" />
                    <h3 className="text-lg font-semibold text-gray-900">Clarification Q&A</h3>
                </div>

                {/* Messages List */}
                <div className="space-y-4 mb-6">
                    {sortedMessages.length === 0 ? (
                        <div className="text-center py-8 text-gray-500">
                            <MessageCircle className="w-12 h-12 mx-auto mb-3 text-gray-300" />
                            <p className="text-sm">No messages yet. Start the conversation!</p>
                        </div>
                    ) : (
                        sortedMessages.map((message, index) => (
                            <div key={message.id || index} className={`border-b border-gray-100 pb-4 last:border-b-0 ${message.isOwnMessage ? 'flex flex-col items-end' : ''}`}>
                                <div className={`${message.isOwnMessage ? 'flex flex-col items-end' : ''} w-full max-w-[80%] ${message.isOwnMessage ? 'ml-auto' : ''}`}>
                                    <div className={`flex justify-between items-start mb-2 w-full ${message.isOwnMessage ? 'flex-row-reverse' : ''}`}>
                                        <div className={`flex items-center gap-3 ${message.isOwnMessage ? 'flex-row-reverse' : ''}`}>
                                            <div className="w-8 h-8 bg-gray-100 rounded-none flex items-center justify-center">
                                                <span className="text-xs font-bold text-gray-600">
                                                    {message.createdByUser?.charAt(0).toUpperCase() || 'U'}
                                                </span>
                                            </div>
                                            <div className={message.isOwnMessage ? 'text-right' : ''}>
                                                <div className={`flex items-center gap-2 ${message.isOwnMessage ? 'flex-row-reverse' : ''}`}>
                                                    <span className="font-medium text-sm text-gray-900">
                                                        {message.createdByUser || message.createdByPartner || 'Unknown'}
                                                    </span>
                                                    {message.createdByPartner && (
                                                        <Badge className="bg-blue-100 text-blue-800 text-xs px-2 py-0.5 rounded-none border-0">
                                                            PARTNER
                                                        </Badge>
                                                    )}
                                                </div>
                                                <div className="text-xs text-gray-500 uppercase tracking-wide">
                                                    {formatTimeAgo(message.createdDate)}
                                                </div>
                                            </div>
                                        </div>


                                        {/* Message Actions */}
                                        {canEdit(message) && (
                                            <div className="flex gap-1">
                                                <Button
                                                    variant="ghost"
                                                    size="sm"
                                                    onClick={() => handleEditMessage(message.id!, message.content || '')}
                                                    className="h-6 w-6 p-0 text-gray-600 hover:text-gray-800 hover:bg-gray-100"
                                                >
                                                    <Edit className="w-3 h-3" />
                                                </Button>
                                                <AlertDialog>
                                                    <AlertDialogTrigger asChild>
                                                        <Button
                                                            variant="ghost"
                                                            size="sm"
                                                            className="h-6 w-6 p-0 text-gray-600 hover:text-red-600 hover:bg-red-50"
                                                        >
                                                            <Trash2 className="w-3 h-3" />
                                                        </Button>
                                                    </AlertDialogTrigger>
                                                    <AlertDialogContent>
                                                        <AlertDialogHeader>
                                                            <AlertDialogTitle>Delete Message</AlertDialogTitle>
                                                            <AlertDialogDescription>
                                                                Are you sure you want to delete this message? This action cannot be undone.
                                                            </AlertDialogDescription>
                                                        </AlertDialogHeader>
                                                        <AlertDialogFooter>
                                                            <AlertDialogCancel>Cancel</AlertDialogCancel>
                                                            <AlertDialogAction
                                                                onClick={() => deleteMessageMutation.mutate(message.id!)}
                                                                className="bg-red-600 hover:bg-red-700 text-white"
                                                            >
                                                                Delete
                                                            </AlertDialogAction>
                                                        </AlertDialogFooter>
                                                    </AlertDialogContent>
                                                </AlertDialog>
                                            </div>
                                        )}
                                    </div>

                                    {/* Message Content */}
                                    {editingMessage === message.id ? (
                                        <div className={`${message.isOwnMessage ? 'mr-11' : 'ml-11'} space-y-2 w-full`}>
                                            <Textarea
                                                value={editContent}
                                                onChange={(e) => setEditContent(e.target.value)}
                                                className="text-sm resize-none rounded-none"
                                                rows={3}
                                            />
                                            <div className={`flex gap-2 ${message.isOwnMessage ? 'justify-end' : ''}`}>
                                                <Button
                                                    size="sm"
                                                    onClick={handleUpdateMessage}
                                                    disabled={updateMessageMutation.isPending}
                                                    className="bg-black text-white hover:bg-gray-800 rounded-none text-xs"
                                                >
                                                    Save
                                                </Button>
                                                <Button
                                                    size="sm"
                                                    variant="outline"
                                                    onClick={() => {
                                                        setEditingMessage(null);
                                                        setEditContent("");
                                                    }}
                                                    className="rounded-none text-xs"
                                                >
                                                    Cancel
                                                </Button>
                                            </div>
                                        </div>
                                    ) : (
                                        <div className={`${message.isOwnMessage ? 'mr-11' : 'ml-11'} group`}>
                                            <p className={`text-sm text-gray-700 leading-relaxed whitespace-pre-wrap ${message.isOwnMessage ? 'text-right' : ''}`}>
                                                {message.content}
                                            </p>
                                        </div>
                                    )}
                                </div>
                            </div>
                        ))
                    )}
                </div>

                {/* Add New Message */}
                {canAddMessage && (
                    <div className="border-t border-gray-100 pt-4">
                        <div className="text-xs text-gray-500 uppercase tracking-wide mb-3">
                            ASK A QUESTION ABOUT THIS OPPORTUNITY
                        </div>
                        <div className="space-y-3">
                            <Textarea
                                value={newMessage}
                                onChange={(e) => setNewMessage(e.target.value)}
                                placeholder="Type your question here..."
                                className="resize-none rounded-none border-gray-200 text-sm"
                                rows={3}
                            />
                            <div className="flex justify-end">
                                <Button
                                    onClick={handleAddMessage}
                                    disabled={!newMessage.trim() || addMessageMutation.isPending}
                                    className="bg-black text-white hover:bg-gray-800 rounded-none text-xs font-medium px-6"
                                >
                                    <Send className="w-3 h-3 mr-2" />
                                    {addMessageMutation.isPending ? 'SENDING...' : 'SEND'}
                                </Button>
                            </div>
                        </div>
                    </div>
                )}
            </div>
        </div>
    );
}
