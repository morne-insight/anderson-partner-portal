using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.QueryHandler", Version = "1.0")]

namespace AndersonAPI.Application.Industries.GetIndustries
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class GetIndustriesQueryHandler : IRequestHandler<GetIndustriesQuery, List<IndustryDto>>
    {
        private readonly IIndustryRepository _industryRepository;

        [IntentManaged(Mode.Merge)]
        public GetIndustriesQueryHandler(IIndustryRepository industryRepository)
        {
            _industryRepository = industryRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task<List<IndustryDto>> Handle(GetIndustriesQuery request, CancellationToken cancellationToken)
        {
            var industries = await _industryRepository.FindAllProjectToAsync<IndustryDto>(cancellationToken);
            return industries;
        }
    }
}