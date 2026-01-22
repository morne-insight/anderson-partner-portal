using AndersonAPI.Domain.Entities;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandHandler", Version = "2.0")]

namespace AndersonAPI.Application.Quarterlies.CreateQuarterly
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class CreateQuarterlyCommandHandler : IRequestHandler<CreateQuarterlyCommand, Guid>
    {
        private readonly IQuarterlyRepository _quarterlyRepository;

        [IntentManaged(Mode.Merge)]
        public CreateQuarterlyCommandHandler(IQuarterlyRepository quarterlyRepository)
        {
            _quarterlyRepository = quarterlyRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Merge)]
        public async Task<Guid> Handle(CreateQuarterlyCommand request, CancellationToken cancellationToken)
        {
            var quarterly = new Quarterly(request.Year, request.Quarter, request.CompanyId, new List<ReportPartner>(), new List<ReportLine>());

            foreach (var report in request.Reports)
            {
                quarterly.AddReportLine(report.PartnerCount, report.Headcount, report.ClientCount, report.OfficeCount, report.LawyerCount, report.EstimatedRevenue, report.CountryId);
            }

            foreach (var partner in request.Partners)
            {
                quarterly.AddReportPartner(partner.Name, partner.Status);
            }

            _quarterlyRepository.Add(quarterly);
            await _quarterlyRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return quarterly.Id;
        }
    }
}