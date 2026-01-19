using AndersonAPI.Domain.Common.Exceptions;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandHandler", Version = "2.0")]

namespace AndersonAPI.Application.Reviews.UpdateReview
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class UpdateReviewCommandHandler : IRequestHandler<UpdateReviewCommand>
    {
        private readonly IReviewRepository _reviewRepository;

        [IntentManaged(Mode.Merge)]
        public UpdateReviewCommandHandler(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task Handle(UpdateReviewCommand request, CancellationToken cancellationToken)
        {
            var review = await _reviewRepository.FindByIdAsync(request.Id, cancellationToken);
            if (review is null)
            {
                throw new NotFoundException($"Could not find Review '{request.Id}'");
            }

            review.Update(request.Comment, request.Rating);
        }
    }
}