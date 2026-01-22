using AndersonAPI.Application.Common.Interfaces;
using AndersonAPI.Domain;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandModels", Version = "1.0")]

namespace AndersonAPI.Application.Quarterlies.AddReportPartnerQuarterly
{
    public class AddReportPartnerQuarterlyCommand : IRequest, ICommand
    {
        public AddReportPartnerQuarterlyCommand(Guid id, string name, PartnerStatus status)
        {
            Id = id;
            Name = name;
            Status = status;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public PartnerStatus Status { get; set; }
    }
}