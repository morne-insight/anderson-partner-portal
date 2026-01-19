using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.QueryHandler", Version = "1.0")]

namespace AndersonAPI.Application.Reviews.GetReviews
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class GetReviewsQueryHandler : IRequestHandler<GetReviewsQuery, List<ReviewDto>>
    {
        private readonly IReviewRepository _reviewRepository;

        [IntentManaged(Mode.Merge)]
        public GetReviewsQueryHandler(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task<List<ReviewDto>> Handle(GetReviewsQuery request, CancellationToken cancellationToken)
        {
            var reviews = await _reviewRepository.FindAllProjectToAsync<ReviewDto>(cancellationToken);
            return reviews;
        }
    }
}