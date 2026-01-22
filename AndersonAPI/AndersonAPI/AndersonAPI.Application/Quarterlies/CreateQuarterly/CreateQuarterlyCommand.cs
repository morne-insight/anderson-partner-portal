using AndersonAPI.Application.Common.Interfaces;
using AndersonAPI.Domain;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandModels", Version = "1.0")]

namespace AndersonAPI.Application.Quarterlies.CreateQuarterly
{
    public class CreateQuarterlyCommand : IRequest<Guid>, ICommand
    {
        public CreateQuarterlyCommand(int year,
            ReportQuarter quarter,
            Guid companyId,
            List<CreateQuarterlyPartnersDto> partners,
            List<CreateQuarterlyReportsDto> reports)
        {
            Year = year;
            Quarter = quarter;
            CompanyId = companyId;
            Partners = partners;
            Reports = reports;
        }

        public int Year { get; set; }

        public ReportQuarter Quarter { get; set; }
        public Guid CompanyId { get; set; }
        public List<CreateQuarterlyPartnersDto> Partners { get; set; }
        public List<CreateQuarterlyReportsDto> Reports { get; set; }
    }
}