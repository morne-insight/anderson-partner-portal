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
        public CreateQuarterlyCommand(int quarter,
            Guid companyId,
            List<CreateQuarterlyPartnersDto> partners,
            List<CreateQuarterlyReportsDto> reports,
            EntityState state)
        {
            Quarter = quarter;
            CompanyId = companyId;
            Partners = partners;
            Reports = reports;
            State = state;
        }

        public int Quarter { get; set; }
        public Guid CompanyId { get; set; }
        public List<CreateQuarterlyPartnersDto> Partners { get; set; }
        public List<CreateQuarterlyReportsDto> Reports { get; set; }
        public EntityState State { get; set; }
    }
}