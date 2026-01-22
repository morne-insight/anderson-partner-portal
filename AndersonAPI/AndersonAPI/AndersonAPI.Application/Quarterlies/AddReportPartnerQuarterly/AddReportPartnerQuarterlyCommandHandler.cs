using AndersonAPI.Domain.Common.Exceptions;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandHandler", Version = "2.0")]

namespace AndersonAPI.Application.Quarterlies.AddReportPartnerQuarterly
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class AddReportPartnerQuarterlyCommandHandler : IRequestHandler<AddReportPartnerQuarterlyCommand>
    {
        private readonly IQuarterlyRepository _quarterlyRepository;

        [IntentManaged(Mode.Merge)]
        public AddReportPartnerQuarterlyCommandHandler(IQuarterlyRepository quarterlyRepository)
        {
            _quarterlyRepository = quarterlyRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task Handle(AddReportPartnerQuarterlyCommand request, CancellationToken cancellationToken)
        {
            var quarterly = await _quarterlyRepository.FindByIdAsync(request.Id, cancellationToken);
            if (quarterly is null)
            {
                throw new NotFoundException($"Could not find Quarterly '{request.Id}'");
            }

            quarterly.AddReportPartner(request.Name, request.Status);
        }
    }
}