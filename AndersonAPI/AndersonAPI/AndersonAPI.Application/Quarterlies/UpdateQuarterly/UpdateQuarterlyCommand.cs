using AndersonAPI.Application.Common.Interfaces;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandModels", Version = "1.0")]

namespace AndersonAPI.Application.Quarterlies.UpdateQuarterly
{
    public class UpdateQuarterlyCommand : IRequest, ICommand
    {
        public UpdateQuarterlyCommand(Guid id, int quarter)
        {
            Id = id;
            Quarter = quarter;
        }

        public Guid Id { get; set; }
        public int Quarter { get; set; }
    }
}