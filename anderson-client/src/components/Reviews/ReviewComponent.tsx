import React, { useState } from 'react';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { Star, Edit2, Trash2, Plus, X, Check, Loader2 } from 'lucide-react';
import { useAuth } from '@/contexts/auth-context';
import {
    type ReviewDto,
    type CreateReviewCommand,
    type UpdateReviewCommand,
} from '@/api';
import { callApi } from '@/server/proxy';

interface ReviewComponentProps {
    partnerId: string;
}

export function ReviewComponent({ partnerId }: ReviewComponentProps) {
    const { user } = useAuth();
    const [showAddForm, setShowAddForm] = useState(false);
    const [editingReview, setEditingReview] = useState<ReviewDto | null>(null);
    const [formData, setFormData] = useState({
        rating: 5,
        comment: ''
    });
    const queryClient = useQueryClient();

    // Check if user can leave a review (must be linked to a company)
    const notLinkedtoAnyCompany = user && user.companies && user.companies.length > 0;
    const linkedToThisCompany = user && user.companies && !user.companies.some(company => company.id === partnerId)
    const canLeaveReview = notLinkedtoAnyCompany && linkedToThisCompany;

    // Fetch reviews using useQuery
    const { data: reviews = [], isLoading, isError } = useQuery({
        queryKey: ['reviews', partnerId],
        queryFn: async () => {
            const response = await callApi({
                data: {
                    fn: 'getApiReviewsCompanyById',
                    args: { path: { id: partnerId } }
                }
            });

            if (response) {
                return response as ReviewDto[];
            }
            return [];
        },
        staleTime: 30000, // Consider data fresh for 30 seconds
        refetchOnWindowFocus: true
    });

    // Create review mutation
    const createReviewMutation = useMutation({
        mutationFn: async (reviewData: { rating: number; comment: string }) => {
            if (!user || !user.companies || user.companies.length === 0) {
                throw new Error('You must be linked to a company to leave a review.');
            }

            const command: CreateReviewCommand = {
                comment: reviewData.comment,
                rating: reviewData.rating,
                applicationIdentityUserId: user.userId || '',
                reviewerCompanyId: user.companies[0].id || '',
                companyId: partnerId
            };

            return callApi({
                data: {
                    fn: 'postApiReviews',
                    args: { body: command }
                }
            });
        },
        onSuccess: () => {
            setFormData({ rating: 5, comment: '' });
            setShowAddForm(false);
            queryClient.invalidateQueries({ queryKey: ['reviews', partnerId] });
        },
        onError: (error) => {
            console.error('Failed to create review:', error);
            alert('Failed to submit review. Please try again.');
        }
    });

    // Update review mutation
    const updateReviewMutation = useMutation({
        mutationFn: async ({ reviewId, rating, comment }: { reviewId: string; rating: number; comment: string }) => {
            const updateData: UpdateReviewCommand = {
                id: reviewId,
                comment,
                rating
            };

            return callApi({
                data: {
                    fn: 'putApiReviewsById',
                    args: { path: { id: reviewId }, body: updateData }
                }
            });
        },
        onSuccess: () => {
            setFormData({ rating: 5, comment: '' });
            setEditingReview(null);
            queryClient.invalidateQueries({ queryKey: ['reviews', partnerId] });
        },
        onError: (error) => {
            console.error('Failed to update review:', error);
            alert('Failed to update review. Please try again.');
        }
    });

    // Delete review mutation
    const deleteReviewMutation = useMutation({
        mutationFn: async (reviewId: string) => {
            return callApi({
                data: {
                    fn: 'deleteApiReviewsById',
                    args: { path: { id: reviewId } }
                }
            });
        },
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['reviews', partnerId] });
        },
        onError: (error) => {
            console.error('Failed to delete review:', error);
            alert('Failed to delete review. Please try again.');
        }
    });

    // Handler functions
    const handleSubmitReview = (e: React.FormEvent) => {
        e.preventDefault();
        createReviewMutation.mutate({
            rating: formData.rating,
            comment: formData.comment
        });
    };

    const handleUpdateReview = (e: React.FormEvent) => {
        e.preventDefault();
        if (!editingReview) return;

        updateReviewMutation.mutate({
            reviewId: editingReview.id!,
            rating: formData.rating,
            comment: formData.comment
        });
    };

    const handleDeleteReview = (reviewId: string) => {
        if (!confirm('Are you sure you want to delete this review?')) {
            return;
        }
        deleteReviewMutation.mutate(reviewId);
    };

    const startEditing = (review: ReviewDto) => {
        // Check if review can be edited (within 5 days)
        if (review.createdDate) {
            const reviewDate = new Date(review.createdDate);
            const fiveDaysAgo = new Date();
            fiveDaysAgo.setDate(fiveDaysAgo.getDate() - 5);

            if (reviewDate < fiveDaysAgo) {
                alert('Reviews can only be edited within 5 days of posting.');
                return;
            }
        }

        setEditingReview(review);
        setFormData({
            rating: review.rating || 5,
            comment: review.comment || ''
        });
    };

    const cancelEditing = () => {
        setEditingReview(null);
        setFormData({ rating: 5, comment: '' });
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

    const renderStars = (rating: number, interactive = false, onRatingChange?: (rating: number) => void) => {
        return (
            <div className="flex items-center gap-1">
                {[1, 2, 3, 4, 5].map((star) => (
                    <Star
                        key={star}
                        className={`w-4 h-4 ${star <= rating
                            ? 'text-yellow-400 fill-yellow-400'
                            : 'text-gray-200'
                            } ${interactive ? 'cursor-pointer hover:text-yellow-300' : ''}`}
                        onClick={interactive && onRatingChange ? () => onRatingChange(star) : undefined}
                    />
                ))}
            </div>
        );
    };

    const renderReviewForm = (isEditing = false) => (
        <form onSubmit={isEditing ? handleUpdateReview : handleSubmitReview} className="space-y-4">
            <div>
                <label className="block text-sm font-bold text-black mb-2 uppercase tracking-wider">
                    Rating
                </label>
                {renderStars(formData.rating, true, (rating) => setFormData({ ...formData, rating }))}
            </div>

            <div>
                <label className="block text-sm font-bold text-black mb-2 uppercase tracking-wider">
                    Comment
                </label>
                <textarea
                    value={formData.comment}
                    onChange={(e) => setFormData({ ...formData, comment: e.target.value })}
                    className="w-full p-3 border border-gray-200 focus:border-red-600 focus:outline-none resize-none"
                    rows={4}
                    placeholder="Share your experience working with this partner..."
                    required
                />
            </div>

            <div className="flex gap-3">
                <button
                    type="submit"
                    disabled={isEditing ? updateReviewMutation.isPending : createReviewMutation.isPending}
                    className="px-6 py-2 bg-red-600 text-white text-xs font-bold uppercase tracking-wider hover:bg-red-700 transition-colors disabled:opacity-50 disabled:cursor-not-allowed"
                >
                    <Check className="w-4 h-4 mr-2 inline" />
                    {isEditing
                        ? (updateReviewMutation.isPending ? 'Updating...' : 'Update Review')
                        : (createReviewMutation.isPending ? 'Submitting...' : 'Submit Review')
                    }
                </button>
                <button
                    type="button"
                    onClick={isEditing ? cancelEditing : () => setShowAddForm(false)}
                    className="px-6 py-2 border border-gray-300 text-gray-600 text-xs font-bold uppercase tracking-wider hover:bg-gray-50 transition-colors"
                >
                    <X className="w-4 h-4 mr-2 inline" />
                    Cancel
                </button>
            </div>
        </form>
    );

    // Show loading state
    if (isLoading) {
        return (
            <div className="bg-white p-8 border border-gray-200 shadow-sm">
                <div className="flex items-center justify-between mb-8 border-b border-gray-100 pb-4">
                    <h3 className="text-xl font-serif text-black flex items-center">
                        <Star className="w-5 h-5 mr-3 text-red-600" />
                        Client Reviews
                    </h3>
                </div>
                <div className="flex justify-center items-center py-8">
                    <Loader2 className="w-6 h-6 animate-spin text-red-600" />
                    <span className="ml-2 text-sm text-gray-500">Loading reviews...</span>
                </div>
            </div>
        );
    }
    console.log("reviews ->:", reviews);
    // Show error state
    if (isError) {
        return (
            <div className="bg-white p-8 border border-gray-200 shadow-sm">
                <div className="flex items-center justify-between mb-8 border-b border-gray-100 pb-4">
                    <h3 className="text-xl font-serif text-black flex items-center">
                        <Star className="w-5 h-5 mr-3 text-red-600" />
                        Client Reviews
                    </h3>
                </div>
                <div className="text-center py-8 text-red-500">
                    <p className="text-sm">Failed to load reviews. Please try again.</p>
                    <button
                        onClick={() => queryClient.invalidateQueries({ queryKey: ['reviews', partnerId] })}
                        className="mt-2 px-4 py-2 border border-gray-300 text-gray-600 text-xs font-bold uppercase tracking-wider hover:bg-gray-50 transition-colors"
                    >
                        Retry
                    </button>
                </div>
            </div>
        );
    }

    return (
        <div className="bg-white p-8 border border-gray-200 shadow-sm">
            <div className="flex items-center justify-between mb-8 border-b border-gray-100 pb-4">
                <h3 className="text-xl font-serif text-black flex items-center">
                    <Star className="w-5 h-5 mr-3 text-red-600" />
                    Client Reviews ({reviews.length})
                </h3>

                {canLeaveReview && !showAddForm && !editingReview && (
                    <button
                        onClick={() => setShowAddForm(true)}
                        className="px-4 py-2 bg-red-600 text-white text-xs font-bold uppercase tracking-wider hover:bg-red-700 transition-colors"
                    >
                        <Plus className="w-4 h-4 mr-2 inline" />
                        Add Review
                    </button>
                )}
            </div>

            {/* Add Review Form */}
            {showAddForm && (
                <div className="mb-8 p-6 bg-gray-50 border border-gray-200">
                    <h4 className="text-lg font-serif text-black mb-4">Leave a Review</h4>
                    {renderReviewForm()}
                </div>
            )}

            {/* Edit Review Form */}
            {editingReview && (
                <div className="mb-8 p-6 bg-blue-50 border border-blue-200">
                    <h4 className="text-lg font-serif text-black mb-4">Edit Review</h4>
                    {renderReviewForm(true)}
                </div>
            )}

            {/* Reviews List */}
            <div className="space-y-6">
                {reviews.length > 0 ? (
                    reviews.map((review) => (
                        <div
                            key={review.id}
                            className="border-l-4 border-gray-100 pl-6 pb-6 border-b last:border-b-0"
                        >
                            <div className="flex items-start justify-between mb-3">
                                <div className="flex items-center gap-3">
                                    {renderStars(review.rating || 0)}
                                    <span className="text-sm text-gray-500">
                                        {review.rating}/5
                                    </span>
                                </div>

                                {canEditReview(review) && (
                                    <div className="flex gap-2">
                                        <button
                                            onClick={() => startEditing(review)}
                                            className="p-1 text-gray-400 hover:text-blue-600 transition-colors"
                                            title="Edit review"
                                        >
                                            <Edit2 className="w-4 h-4" />
                                        </button>
                                        <button
                                            onClick={() => handleDeleteReview(review.id!)}
                                            className="p-1 text-gray-400 hover:text-red-600 transition-colors"
                                            title="Delete review"
                                        >
                                            <Trash2 className="w-4 h-4" />
                                        </button>
                                    </div>
                                )}
                            </div>

                            <p className="text-gray-600 font-light italic mb-4 leading-relaxed">
                                "{review.comment}"
                            </p>

                            <div className="flex justify-between items-center text-xs font-bold uppercase tracking-widest text-gray-400">
                                <span>{review.reviewerCompanyName || 'Anonymous Company'}</span>
                                <span>
                                    {review.createdDate
                                        ? new Date(review.createdDate).toLocaleDateString()
                                        : 'Recently'
                                    }
                                </span>
                            </div>
                        </div>
                    ))
                ) : (
                    <div className="text-center py-12">
                        <Star className="w-12 h-12 text-gray-300 mx-auto mb-4" />
                        <p className="text-gray-400 italic">No reviews yet.</p>
                        {canLeaveReview && (
                            <p className="text-sm text-gray-500 mt-2">
                                Be the first to share your experience with this partner.
                            </p>
                        )}
                    </div>
                )}
            </div>

            {/* Info message for users without company access */}
            {!notLinkedtoAnyCompany && (
                <div className="mt-6 p-4 bg-yellow-50 border border-yellow-200 rounded">
                    <p className="text-sm text-yellow-800">
                        You must be linked to a company to leave reviews.
                    </p>
                </div>
            )}
        </div>
    );
}
