import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { Edit, Loader2, MessageCircle, Send, Trash2 } from "lucide-react";
import { useState } from "react";
import { toast } from "sonner";
import type {
  AddMessageOpportunityCommand,
  OpportunityMessageDto,
  UpdateMessageOpportunityCommand,
} from "@/api";
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
import { Badge } from "@/components/ui/badge";
import { Button } from "@/components/ui/button";
import { Textarea } from "@/components/ui/textarea";
import { useAuth } from "@/contexts/auth-context";
import { callApi } from "@/server/proxy";

interface OpportunityMessagesProps {
  opportunityId: string;
  canAddMessage?: boolean;
  isOwnOpportunity?: boolean;
}

export function OpportunityMessages({
  opportunityId,
  canAddMessage = true,
  isOwnOpportunity = false,
}: OpportunityMessagesProps) {
  const { user } = useAuth();
  const [newMessage, setNewMessage] = useState("");
  const [editingMessage, setEditingMessage] = useState<string | null>(null);
  const [editContent, setEditContent] = useState("");
  const queryClient = useQueryClient();

  // Fetch messages using the new endpoint
  const {
    data: messages = [],
    isLoading,
    isError,
  } = useQuery({
    queryKey: ["opportunity-messages", opportunityId],
    queryFn: () =>
      callApi({
        data: {
          fn: "getApiOpportunitiesByIdMessages",
          args: {
            path: { id: opportunityId },
          },
        },
      }),
    staleTime: 30_000, // Consider data fresh for 30 seconds
    refetchOnWindowFocus: true,
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
        isOwnMessage: isOwnOpportunity,
      };

      return callApi({
        data: { fn: "postApiOpportunitiesMessage", args: { body: command } },
      });
    },
    onSuccess: () => {
      setNewMessage("");
      // Invalidate the messages query to refetch
      queryClient.invalidateQueries({
        queryKey: ["opportunity-messages", opportunityId],
      });
    },
    onError: (error) => {
      console.error("Failed to add message:", error);
      toast.error("Failed to send message. Please try again.");
    },
  });

  const updateMessageMutation = useMutation({
    mutationFn: async ({
      messageId,
      content,
    }: {
      messageId: string;
      content: string;
    }) => {
      const command: UpdateMessageOpportunityCommand = {
        id: opportunityId,
        messageId,
        content,
      };
      return await callApi({
        data: {
          fn: "putApiOpportunitiesByIdMessage",
          args: { path: { id: opportunityId }, body: command },
        },
      });
    },
    onSuccess: () => {
      setEditingMessage(null);
      setEditContent("");
      queryClient.invalidateQueries({
        queryKey: ["opportunity-messages", opportunityId],
      });
    },
    onError: (error) => {
      console.error("Failed to update message:", error);
      toast.error("Failed to update message. Please try again.");
    },
  });

  const deleteMessageMutation = useMutation({
    mutationFn: async (messageId: string) => {
      return callApi({
        data: {
          fn: "deleteApiOpportunitiesByIdMessage",
          args: {
            path: { id: opportunityId },
            query: { messageId },
          },
        },
      });
    },
    onSuccess: () => {
      queryClient.invalidateQueries({
        queryKey: ["opportunity-messages", opportunityId],
      });
    },
    onError: (error) => {
      console.error("Failed to delete message:", error);
      toast.error("Failed to delete message. Please try again.");
    },
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
        content: editContent.trim(),
      });
    }
  };

  const formatTimeAgo = (date: Date | string | undefined) => {
    if (!date) return "";
    const messageDate = new Date(date);
    const now = new Date();
    const diffInHours = Math.floor(
      (now.getTime() - messageDate.getTime()) / (1000 * 60 * 60)
    );

    if (diffInHours < 1) return "JUST NOW";
    if (diffInHours === 1) return "1 HOUR AGO";
    if (diffInHours < 24) return `${diffInHours} HOURS AGO`;

    const diffInDays = Math.floor(diffInHours / 24);
    if (diffInDays === 1) return "1 DAY AGO";
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
      <div className="rounded-none border border-gray-200 bg-white">
        <div className="p-6">
          <div className="mb-6 flex items-center gap-2">
            <MessageCircle className="h-5 w-5 text-red-500" />
            <h3 className="font-semibold text-gray-900 text-lg">
              Clarification Q&A
            </h3>
          </div>
          <div className="flex items-center justify-center py-8">
            <Loader2 className="h-6 w-6 animate-spin text-gray-400" />
            <span className="ml-2 text-gray-500 text-sm">
              Loading messages...
            </span>
          </div>
        </div>
      </div>
    );
  }

  // Show error state
  if (isError) {
    return (
      <div className="rounded-none border border-gray-200 bg-white">
        <div className="p-6">
          <div className="mb-6 flex items-center gap-2">
            <MessageCircle className="h-5 w-5 text-red-500" />
            <h3 className="font-semibold text-gray-900 text-lg">
              Clarification Q&A
            </h3>
          </div>
          <div className="py-8 text-center text-red-500">
            <p className="text-sm">
              Failed to load messages. Please try again.
            </p>
            <Button
              className="mt-2"
              onClick={() =>
                queryClient.invalidateQueries({
                  queryKey: ["opportunity-messages", opportunityId],
                })
              }
              size="sm"
              variant="outline"
            >
              Retry
            </Button>
          </div>
        </div>
      </div>
    );
  }

  return (
    <div className="rounded-none border border-gray-200 bg-white">
      <div className="p-6">
        <div className="mb-6 flex items-center gap-2">
          <MessageCircle className="h-5 w-5 text-red-500" />
          <h3 className="font-semibold text-gray-900 text-lg">
            Clarification Q&A
          </h3>
        </div>

        {/* Messages List */}
        <div className="mb-6 space-y-4">
          {sortedMessages.length === 0 ? (
            <div className="py-8 text-center text-gray-500">
              <MessageCircle className="mx-auto mb-3 h-12 w-12 text-gray-300" />
              <p className="text-sm">
                No messages yet. Start the conversation!
              </p>
            </div>
          ) : (
            sortedMessages.map((message, index) => (
              <div
                className={`border-gray-100 border-b pb-4 last:border-b-0 ${message.isOwnMessage ? "flex flex-col items-end" : ""}`}
                key={message.id || index}
              >
                <div
                  className={`${message.isOwnMessage ? "flex flex-col items-end" : ""} w-full max-w-[80%] ${message.isOwnMessage ? "ml-auto" : ""}`}
                >
                  <div
                    className={`mb-2 flex w-full items-start justify-between ${message.isOwnMessage ? "flex-row-reverse" : ""}`}
                  >
                    <div
                      className={`flex items-center gap-3 ${message.isOwnMessage ? "flex-row-reverse" : ""}`}
                    >
                      <div className="flex h-8 w-8 items-center justify-center rounded-none bg-gray-100">
                        <span className="font-bold text-gray-600 text-xs">
                          {message.createdByUser?.charAt(0).toUpperCase() ||
                            "U"}
                        </span>
                      </div>
                      <div className={message.isOwnMessage ? "text-right" : ""}>
                        <div
                          className={`flex items-center gap-2 ${message.isOwnMessage ? "flex-row-reverse" : ""}`}
                        >
                          <span className="font-medium text-gray-900 text-sm">
                            {message.createdByUser ||
                              message.createdByPartner ||
                              "Unknown"}
                          </span>
                          {message.createdByPartner && (
                            <Badge className="rounded-none border-0 bg-blue-100 px-2 py-0.5 text-blue-800 text-xs">
                              PARTNER
                            </Badge>
                          )}
                        </div>
                        <div className="text-gray-500 text-xs uppercase tracking-wide">
                          {formatTimeAgo(message.createdDate)}
                        </div>
                      </div>
                    </div>

                    {/* Message Actions */}
                    {canEdit(message) && (
                      <div className="flex gap-1">
                        <Button
                          className="h-6 w-6 p-0 text-gray-600 hover:bg-gray-100 hover:text-gray-800"
                          onClick={() =>
                            handleEditMessage(
                              message.id!,
                              message.content || ""
                            )
                          }
                          size="sm"
                          variant="ghost"
                        >
                          <Edit className="h-3 w-3" />
                        </Button>
                        <AlertDialog>
                          <AlertDialogTrigger asChild>
                            <Button
                              className="h-6 w-6 p-0 text-gray-600 hover:bg-red-50 hover:text-red-600"
                              size="sm"
                              variant="ghost"
                            >
                              <Trash2 className="h-3 w-3" />
                            </Button>
                          </AlertDialogTrigger>
                          <AlertDialogContent>
                            <AlertDialogHeader>
                              <AlertDialogTitle>
                                Delete Message
                              </AlertDialogTitle>
                              <AlertDialogDescription>
                                Are you sure you want to delete this message?
                                This action cannot be undone.
                              </AlertDialogDescription>
                            </AlertDialogHeader>
                            <AlertDialogFooter>
                              <AlertDialogCancel>Cancel</AlertDialogCancel>
                              <AlertDialogAction
                                className="bg-red-600 text-white hover:bg-red-700"
                                onClick={() =>
                                  deleteMessageMutation.mutate(message.id!)
                                }
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
                    <div
                      className={`${message.isOwnMessage ? "mr-11" : "ml-11"} w-full space-y-2`}
                    >
                      <Textarea
                        className="resize-none rounded-none text-sm"
                        onChange={(e) => setEditContent(e.target.value)}
                        rows={3}
                        value={editContent}
                      />
                      <div
                        className={`flex gap-2 ${message.isOwnMessage ? "justify-end" : ""}`}
                      >
                        <Button
                          className="rounded-none bg-black text-white text-xs hover:bg-gray-800"
                          disabled={updateMessageMutation.isPending}
                          onClick={handleUpdateMessage}
                          size="sm"
                        >
                          Save
                        </Button>
                        <Button
                          className="rounded-none text-xs"
                          onClick={() => {
                            setEditingMessage(null);
                            setEditContent("");
                          }}
                          size="sm"
                          variant="outline"
                        >
                          Cancel
                        </Button>
                      </div>
                    </div>
                  ) : (
                    <div
                      className={`${message.isOwnMessage ? "mr-11" : "ml-11"} group`}
                    >
                      <p
                        className={`whitespace-pre-wrap text-gray-700 text-sm leading-relaxed ${message.isOwnMessage ? "text-right" : ""}`}
                      >
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
          <div className="border-gray-100 border-t pt-4">
            <div className="mb-3 text-gray-500 text-xs uppercase tracking-wide">
              ASK A QUESTION ABOUT THIS OPPORTUNITY
            </div>
            <div className="space-y-3">
              <Textarea
                className="resize-none rounded-none border-gray-200 text-sm"
                onChange={(e) => setNewMessage(e.target.value)}
                placeholder="Type your question here..."
                rows={3}
                value={newMessage}
              />
              <div className="flex justify-end">
                <Button
                  className="rounded-none bg-black px-6 font-medium text-white text-xs hover:bg-gray-800"
                  disabled={!newMessage.trim() || addMessageMutation.isPending}
                  onClick={handleAddMessage}
                >
                  <Send className="mr-2 h-3 w-3" />
                  {addMessageMutation.isPending ? "SENDING..." : "SEND"}
                </Button>
              </div>
            </div>
          </div>
        )}
      </div>
    </div>
  );
}
