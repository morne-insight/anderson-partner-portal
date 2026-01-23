using AndersonAPI.Application.Common.Interfaces;
using AndersonAPI.Domain;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandModels", Version = "1.0")]

namespace AndersonAPI.Application.Quarterlies.UpdateQuarterly
{
    public class UpdateQuarterlyCommand : IRequest, ICommand
    {
        public UpdateQuarterlyCommand(Guid id, int year, ReportQuarter quarter, bool isSubmitted)
        {
            Id = id;
            Year = year;
            Quarter = quarter;
            IsSubmitted = isSubmitted;
        }

        public Guid Id { get; set; }
        public int Year { get; set; }
        public ReportQuarter Quarter { get; set; }
        public bool IsSubmitted { get; set; }
    }
}