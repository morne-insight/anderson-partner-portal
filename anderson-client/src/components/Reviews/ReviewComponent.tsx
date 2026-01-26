import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { Check, Edit2, Loader2, Plus, Star, Trash2, X } from "lucide-react";
import type React from "react";
import { useState } from "react";
import { toast } from "sonner";
import type {
    CreateReviewCommand,
    ReviewDto,
    UpdateReviewCommand,
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
import { useAuth } from "@/contexts/auth-context";
import { callApi } from "@/server/proxy";

interface ReviewComponentProps {
    partnerId: string;
}

export function ReviewComponent({ partnerId }: ReviewComponentProps) {
    const { user } = useAuth();
    const [showAddForm, setShowAddForm] = useState(false);
    const [editingReview, setEditingReview] = useState<ReviewDto | null>(null);
    const [formData, setFormData] = useState({
        rating: 5,
        comment: "",
    });
    const queryClient = useQueryClient();

    // Check if user can leave a review (must be linked to a company)
    const notLinkedtoAnyCompany = user?.companies && user.companies.length > 0;
    const linkedToThisCompany =
        user?.companies &&
        !user.companies.some((company) => company.id === partnerId);
    const canLeaveReview = notLinkedtoAnyCompany && linkedToThisCompany;

    // Fetch reviews using useQuery
    const {
        data: reviews = [],
        isLoading,
        isError,
    } = useQuery({
        queryKey: ["reviews", partnerId],
        queryFn: async () => {
            const response = await callApi({
                data: {
                    fn: "getApiReviewsCompanyById",
                    args: { path: { id: partnerId } },
                },
            });

            if (response) {
                return response as ReviewDto[];
            }
            return [];
        },
        staleTime: 30_000, // Consider data fresh for 30 seconds
        refetchOnWindowFocus: true,
    });

    // Create review mutation
    const createReviewMutation = useMutation({
        mutationFn: async (reviewData: { rating: number; comment: string }) => {
            if (!(user && user.companies) || user.companies.length === 0) {
                throw new Error("You must be linked to a company to leave a review.");
            }

            const command: CreateReviewCommand = {
                comment: reviewData.comment,
                rating: reviewData.rating,
                applicationIdentityUserId: user.userId || "",
                reviewerCompanyId: user.companies[0].id || "",
                companyId: partnerId,
            };

            return await callApi({
                data: {
                    fn: "postApiReviews",
                    args: { body: command },
                },
            });
        },
        onSuccess: () => {
            setFormData({ rating: 5, comment: "" });
            setShowAddForm(false);
            queryClient.invalidateQueries({ queryKey: ["reviews", partnerId] });
        },
        onError: (error) => {
            console.error("Failed to create review:", error);
            toast.error("Failed to submit review. Please try again.");
        },
    });

    // Update review mutation
    const updateReviewMutation = useMutation({
        mutationFn: async ({
            reviewId,
            rating,
            comment,
        }: {
            reviewId: string;
            rating: number;
            comment: string;
        }) => {
            const updateData: UpdateReviewCommand = {
                id: reviewId,
                comment,
                rating,
            };

            return await callApi({
                data: {
                    fn: "putApiReviewsById",
                    args: { path: { id: reviewId }, body: updateData },
                },
            });
        },
        onSuccess: () => {
            setFormData({ rating: 5, comment: "" });
            setEditingReview(null);
            queryClient.invalidateQueries({ queryKey: ["reviews", partnerId] });
        },
        onError: (error) => {
            console.error("Failed to update review:", error);
            toast.error("Failed to update review. Please try again.");
        },
    });

    // Delete review mutation
    const deleteReviewMutation = useMutation({
        mutationFn: async (reviewId: string) => {
            return await callApi({
                data: {
                    fn: "deleteApiReviewsById",
                    args: { path: { id: reviewId } },
                },
            });
        },
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ["reviews", partnerId] });
        },
        onError: (error) => {
            console.error("Failed to delete review:", error);
            toast.error("Failed to delete review. Please try again.");
        },
    });

    // Handler functions
    const handleSubmitReview = (e: React.FormEvent) => {
        e.preventDefault();
        createReviewMutation.mutate({
            rating: formData.rating,
            comment: formData.comment,
        });
    };

    const handleUpdateReview = (e: React.FormEvent) => {
        e.preventDefault();
        if (!editingReview) return;

        updateReviewMutation.mutate({
            reviewId: editingReview.id!,
            rating: formData.rating,
            comment: formData.comment,
        });
    };

    const startEditing = (review: ReviewDto) => {
        // Check if review can be edited (within 5 days)
        if (review.createdDate) {
            const reviewDate = new Date(review.createdDate);
            const fiveDaysAgo = new Date();
            fiveDaysAgo.setDate(fiveDaysAgo.getDate() - 5);

            if (reviewDate < fiveDaysAgo) {
                toast.warning("Reviews can only be edited within 5 days of posting.");
                return;
            }
        }

        setEditingReview(review);
        setFormData({
            rating: review.rating || 5,
            comment: review.comment || "",
        });
    };

    const cancelEditing = () => {
        setEditingReview(null);
        setFormData({ rating: 5, comment: "" });
    };

    const canEditReview = (review: ReviewDto) => {
        // Check if current user owns the review
        if (review.userId !== user?.userId) {
            return false;
        }

        // Check if within 5 days
        if (review.createdDate) {
            const reviewDate = new Date(review.createdDate);
            const fiveDaysAgo = new Date();
            fiveDaysAgo.setDate(fiveDaysAgo.getDate() - 5);
            return reviewDate >= fiveDaysAgo;
        }

        return true;
    };

    const renderStars = (
        rating: number,
        interactive = false,
        onRatingChange?: (rating: number) => void
    ) => {
        return (
            <div className="flex items-center gap-1">
                <label
                    className="mb-2 block font-bold text-black text-sm uppercase tracking-wider"
                    htmlFor="star-rating"
                >
                    Rating
                </label>
                {[1, 2, 3, 4, 5].map((star) => (
                    <Star
                        className={`h-4 w-4 ${star <= rating
                            ? "fill-yellow-400 text-yellow-400"
                            : "text-gray-200"
                            } ${interactive ? "cursor-pointer hover:text-yellow-300" : ""}`}
                        id="star-rating"
                        key={star}
                        onClick={
                            interactive && onRatingChange
                                ? () => onRatingChange(star)
                                : undefined
                        }
                    />
                ))}
            </div>
        );
    };

    const renderReviewForm = (isEditing = false) => (
        <form
            className="space-y-4"
            onSubmit={isEditing ? handleUpdateReview : handleSubmitReview}
        >
            <div>
                {renderStars(formData.rating, true, (rating) =>
                    setFormData({ ...formData, rating })
                )}
            </div>

            <div>
                <label
                    className="mb-2 block font-bold text-black text-sm uppercase tracking-wider"
                    htmlFor="review-comment"
                >
                    Comment
                </label>
                <textarea
                    className="w-full resize-none border border-gray-200 p-3 focus:border-red-600 focus:outline-none"
                    id="review-comment"
                    onChange={(e) =>
                        setFormData({ ...formData, comment: e.target.value })
                    }
                    placeholder="Share your experience working with this partner..."
                    required
                    rows={4}
                    value={formData.comment}
                />
            </div>

            <div className="flex gap-3">
                <button
                    className="bg-red-600 px-6 py-2 font-bold text-white text-xs uppercase tracking-wider transition-colors hover:bg-red-700 disabled:cursor-not-allowed disabled:opacity-50"
                    disabled={
                        isEditing
                            ? updateReviewMutation.isPending
                            : createReviewMutation.isPending
                    }
                    type="submit"
                >
                    <Check className="mr-2 inline h-4 w-4" />
                    {isEditing
                        ? updateReviewMutation.isPending
                            ? "Updating..."
                            : "Update Review"
                        : createReviewMutation.isPending
                            ? "Submitting..."
                            : "Submit Review"}
                </button>
                <button
                    className="border border-gray-300 px-6 py-2 font-bold text-gray-600 text-xs uppercase tracking-wider transition-colors hover:bg-gray-50"
                    onClick={isEditing ? cancelEditing : () => setShowAddForm(false)}
                    type="button"
                >
                    <X className="mr-2 inline h-4 w-4" />
                    Cancel
                </button>
            </div>
        </form>
    );

    // Show loading state
    if (isLoading) {
        return (
            <div className="border border-gray-200 bg-white p-8 shadow-sm">
                <div className="mb-8 flex items-center justify-between border-gray-100 border-b pb-4">
                    <h3 className="flex items-center font-serif text-black text-xl">
                        <Star className="mr-3 h-5 w-5 text-red-600" />
                        Client Reviews
                    </h3>
                </div>
                <div className="flex items-center justify-center py-8">
                    <Loader2 className="h-6 w-6 animate-spin text-red-600" />
                    <span className="ml-2 text-gray-500 text-sm">Loading reviews...</span>
                </div>
            </div>
        );
    }

    // Show error state
    if (isError) {
        return (
            <div className="border border-gray-200 bg-white p-8 shadow-sm">
                <div className="mb-8 flex items-center justify-between border-gray-100 border-b pb-4">
                    <h3 className="flex items-center font-serif text-black text-xl">
                        <Star className="mr-3 h-5 w-5 text-red-600" />
                        Client Reviews
                    </h3>
                </div>
                <div className="py-8 text-center text-red-500">
                    <p className="text-sm">Failed to load reviews. Please try again.</p>
                    <button
                        className="mt-2 border border-gray-300 px-4 py-2 font-bold text-gray-600 text-xs uppercase tracking-wider transition-colors hover:bg-gray-50"
                        onClick={() =>
                            queryClient.invalidateQueries({
                                queryKey: ["reviews", partnerId],
                            })
                        }
                        type="button"
                    >
                        Retry
                    </button>
                </div>
            </div>
        );
    }

    return (
        <div className="border border-gray-200 bg-white p-8 shadow-sm">
            <div className="mb-8 flex items-center justify-between border-gray-100 border-b pb-4">
                <h3 className="flex items-center font-serif text-black text-xl">
                    <Star className="mr-3 h-5 w-5 text-red-600" />
                    Client Reviews ({reviews.length})
                </h3>

                {canLeaveReview && !showAddForm && !editingReview && (
                    <button
                        className="bg-red-600 px-4 py-2 font-bold text-white text-xs uppercase tracking-wider transition-colors hover:bg-red-700"
                        onClick={() => setShowAddForm(true)}
                        type="button"
                    >
                        <Plus className="mr-2 inline h-4 w-4" />
                        Add Review
                    </button>
                )}
            </div>

            {/* Add Review Form */}
            {showAddForm && (
                <div className="mb-8 border border-gray-200 bg-gray-50 p-6">
                    <h4 className="mb-4 font-serif text-black text-lg">Leave a Review</h4>
                    {renderReviewForm()}
                </div>
            )}

            {/* Edit Review Form */}
            {editingReview && (
                <div className="mb-8 border border-blue-200 bg-blue-50 p-6">
                    <h4 className="mb-4 font-serif text-black text-lg">Edit Review</h4>
                    {renderReviewForm(true)}
                </div>
            )}

            {/* Reviews List */}
            <div className="space-y-6">
                {reviews.length > 0 ? (
                    reviews.map((review) => (
                        <div
                            className="border-gray-100 border-b border-l-4 pb-6 pl-6 last:border-b-0"
                            key={review.id}
                        >
                            <div className="mb-3 flex items-start justify-between">
                                <div className="flex items-center gap-3">
                                    {renderStars(review.rating || 0)}
                                    <span className="text-gray-500 text-sm">
                                        {review.rating}/5
                                    </span>
                                </div>

                                {canEditReview(review) && (
                                    <div className="flex gap-2">
                                        <button
                                            className="p-1 text-gray-400 transition-colors hover:text-blue-600"
                                            onClick={() => startEditing(review)}
                                            title="Edit review"
                                            type="button"
                                        >
                                            <Edit2 className="h-4 w-4" />
                                        </button>
                                        <AlertDialog>
                                            <AlertDialogTrigger asChild>
                                                <button
                                                    className="p-1 text-gray-400 transition-colors hover:text-red-600"
                                                    title="Delete review"
                                                    type="button"
                                                >
                                                    <Trash2 className="h-4 w-4" />
                                                </button>
                                            </AlertDialogTrigger>
                                            <AlertDialogContent>
                                                <AlertDialogHeader>
                                                    <AlertDialogTitle>Delete Review</AlertDialogTitle>
                                                    <AlertDialogDescription>
                                                        Are you sure you want to delete this review? This
                                                        action cannot be undone.
                                                    </AlertDialogDescription>
                                                </AlertDialogHeader>
                                                <AlertDialogFooter>
                                                    <AlertDialogCancel>Cancel</AlertDialogCancel>
                                                    <AlertDialogAction
                                                        className="bg-red-600 font-bold hover:bg-red-700"
                                                        onClick={() =>
                                                            deleteReviewMutation.mutate(review.id!)
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

                            <p className="mb-4 font-light text-gray-600 italic leading-relaxed">
                                "{review.comment}"
                            </p>

                            <div className="flex items-center justify-between font-bold text-gray-400 text-xs uppercase tracking-widest">
                                <span>{review.reviewerCompanyName || "Anonymous Company"}</span>
                                <span>
                                    {review.createdDate
                                        ? new Date(review.createdDate).toLocaleDateString()
                                        : "Recently"}
                                </span>
                            </div>
                        </div>
                    ))
                ) : (
                    <div className="py-12 text-center">
                        <Star className="mx-auto mb-4 h-12 w-12 text-gray-300" />
                        <p className="text-gray-400 italic">No reviews yet.</p>
                        {canLeaveReview && (
                            <p className="mt-2 text-gray-500 text-sm">
                                Be the first to share your experience with this partner.
                            </p>
                        )}
                    </div>
                )}
            </div>

            {/* Info message for users without company access */}
            {!notLinkedtoAnyCompany && (
                <div className="mt-6 rounded border border-yellow-200 bg-yellow-50 p-4">
                    <p className="text-sm text-yellow-800">
                        You must be linked to a company to leave reviews.
                    </p>
                </div>
            )}
        </div>
    );
}
