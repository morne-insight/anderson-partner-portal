using AndersonAPI.Domain.Entities;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandHandler", Version = "2.0")]

namespace AndersonAPI.Application.Reviews.CreateReview
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand, Guid>
    {
        private readonly IReviewRepository _reviewRepository;

        [IntentManaged(Mode.Merge)]
        public CreateReviewCommandHandler(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task<Guid> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
        {
            var review = new Review(
                comment: request.Comment,
                rating: request.Rating,
                applicationIdentityUserId: request.ApplicationIdentityUserId,
                reviewerCompanyId: request.ReviewerCompanyId,
                state: request.State);

            _reviewRepository.Add(review);
            await _reviewRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return review.Id;
        }
    }
}