using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.QueryHandler", Version = "1.0")]

namespace AndersonAPI.Application.Reviews.GetReviewsForCompanyById
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class GetReviewsForCompanyByIdQueryHandler : IRequestHandler<GetReviewsForCompanyByIdQuery, List<ReviewDto>>
    {
        private readonly IReviewRepository _reviewRepository;

        [IntentManaged(Mode.Merge)]
        public GetReviewsForCompanyByIdQueryHandler(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public async Task<List<ReviewDto>> Handle(
            GetReviewsForCompanyByIdQuery request,
            CancellationToken cancellationToken)
        {
            var reviews = await _reviewRepository.FindAllProjectToAsync<ReviewDto>(
                    x => x.CompanyId == request.Id, cancellationToken);

            return reviews;
        }
    }
}