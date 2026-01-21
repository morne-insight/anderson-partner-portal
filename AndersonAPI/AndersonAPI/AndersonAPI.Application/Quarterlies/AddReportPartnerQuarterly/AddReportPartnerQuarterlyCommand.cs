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
        public AddReportPartnerQuarterlyCommand(Guid id, Guid quaterlyId, string name, EntityState state)
        {
            Id = id;
            QuaterlyId = quaterlyId;
            Name = name;
            State = state;
        }

        public Guid Id { get; set; }
        public Guid QuaterlyId { get; set; }
        public string Name { get; set; }
        public EntityState State { get; set; }
    }
}