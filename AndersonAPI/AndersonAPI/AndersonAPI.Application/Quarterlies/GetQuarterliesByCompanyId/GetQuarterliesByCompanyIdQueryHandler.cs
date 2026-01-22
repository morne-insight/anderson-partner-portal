using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.QueryHandler", Version = "1.0")]

namespace AndersonAPI.Application.Quarterlies.GetQuarterliesByCompanyId
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class GetQuarterliesByCompanyIdQueryHandler : IRequestHandler<GetQuarterliesByCompanyIdQuery, List<QuarterlyDto>>
    {
        private readonly IQuarterlyRepository _quarterlyRepository;

        [IntentManaged(Mode.Merge)]
        public GetQuarterliesByCompanyIdQueryHandler(IQuarterlyRepository quarterlyRepository)
        {
            _quarterlyRepository = quarterlyRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task<List<QuarterlyDto>> Handle(
            GetQuarterliesByCompanyIdQuery request,
            CancellationToken cancellationToken)
        {
            var quarterlies = await _quarterlyRepository.FindAllProjectToAsync<QuarterlyDto>(x => x.CompanyId == request.Id, cancellationToken);
            return quarterlies;
        }
    }
}