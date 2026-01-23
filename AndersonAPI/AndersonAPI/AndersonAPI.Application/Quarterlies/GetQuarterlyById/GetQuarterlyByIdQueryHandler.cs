using AndersonAPI.Domain.Common.Exceptions;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.QueryHandler", Version = "1.0")]

namespace AndersonAPI.Application.Quarterlies.GetQuarterlyById
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class GetQuarterlyByIdQueryHandler : IRequestHandler<GetQuarterlyByIdQuery, QuarterlyReportDto>
    {
        private readonly IQuarterlyRepository _quarterlyRepository;

        [IntentManaged(Mode.Merge)]
        public GetQuarterlyByIdQueryHandler(IQuarterlyRepository quarterlyRepository)
        {
            _quarterlyRepository = quarterlyRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task<QuarterlyReportDto> Handle(GetQuarterlyByIdQuery request, CancellationToken cancellationToken)
        {
            var quarterly = await _quarterlyRepository.FindByIdProjectToAsync<QuarterlyReportDto>(request.Id, cancellationToken);
            if (quarterly is null)
            {
                throw new NotFoundException($"Could not find Quarterly '{request.Id}'");
            }
            return quarterly;
        }
    }
}