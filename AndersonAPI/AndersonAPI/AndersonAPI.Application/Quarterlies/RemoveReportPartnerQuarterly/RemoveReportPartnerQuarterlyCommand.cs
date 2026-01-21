using AndersonAPI.Application.Common.Interfaces;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandModels", Version = "1.0")]

namespace AndersonAPI.Application.Quarterlies.RemoveReportPartnerQuarterly
{
    public class RemoveReportPartnerQuarterlyCommand : IRequest, ICommand
    {
        public RemoveReportPartnerQuarterlyCommand(Guid id, Guid reportPartnerId)
        {
            Id = id;
            ReportPartnerId = reportPartnerId;
        }

        public Guid Id { get; set; }
        public Guid ReportPartnerId { get; set; }
    }
}