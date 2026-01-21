using AndersonAPI.Application.Common.Interfaces;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandModels", Version = "1.0")]

namespace AndersonAPI.Application.Quarterlies.UpdateReporPartnerQuarterly
{
    public class UpdateReporPartnerQuarterlyCommand : IRequest, ICommand
    {
        public UpdateReporPartnerQuarterlyCommand(Guid id, Guid reportPartnerId, string name)
        {
            Id = id;
            ReportPartnerId = reportPartnerId;
            Name = name;
        }

        public Guid Id { get; set; }
        public Guid ReportPartnerId { get; set; }
        public string Name { get; set; }
    }
}