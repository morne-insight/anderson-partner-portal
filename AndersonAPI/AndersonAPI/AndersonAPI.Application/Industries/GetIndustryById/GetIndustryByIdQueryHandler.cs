using AndersonAPI.Domain.Common.Exceptions;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.QueryHandler", Version = "1.0")]

namespace AndersonAPI.Application.Industries.GetIndustryById
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class GetIndustryByIdQueryHandler : IRequestHandler<GetIndustryByIdQuery, IndustryDto>
    {
        private readonly IIndustryRepository _industryRepository;

        [IntentManaged(Mode.Merge)]
        public GetIndustryByIdQueryHandler(IIndustryRepository industryRepository)
        {
            _industryRepository = industryRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task<IndustryDto> Handle(GetIndustryByIdQuery request, CancellationToken cancellationToken)
        {
            var industry = await _industryRepository.FindByIdProjectToAsync<IndustryDto>(request.Id, cancellationToken);
            if (industry is null)
            {
                throw new NotFoundException($"Could not find Industry '{request.Id}'");
            }
            return industry;
        }
    }
}