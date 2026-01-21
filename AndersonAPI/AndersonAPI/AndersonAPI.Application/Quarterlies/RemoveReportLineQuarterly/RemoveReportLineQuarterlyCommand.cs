using AndersonAPI.Application.Common.Interfaces;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandModels", Version = "1.0")]

namespace AndersonAPI.Application.Quarterlies.RemoveReportLineQuarterly
{
    public class RemoveReportLineQuarterlyCommand : IRequest, ICommand
    {
        public RemoveReportLineQuarterlyCommand(Guid id, Guid reportLineId)
        {
            Id = id;
            ReportLineId = reportLineId;
        }

        public Guid Id { get; set; }
        public Guid ReportLineId { get; set; }
    }
}