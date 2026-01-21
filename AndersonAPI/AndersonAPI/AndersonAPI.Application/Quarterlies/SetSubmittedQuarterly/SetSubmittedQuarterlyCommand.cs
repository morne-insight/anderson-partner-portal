using AndersonAPI.Application.Common.Interfaces;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandModels", Version = "1.0")]

namespace AndersonAPI.Application.Quarterlies.SetSubmittedQuarterly
{
    public class SetSubmittedQuarterlyCommand : IRequest, ICommand
    {
        public SetSubmittedQuarterlyCommand(Guid id, bool isSubmitted)
        {
            Id = id;
            IsSubmitted = isSubmitted;
        }

        public Guid Id { get; set; }
        public bool IsSubmitted { get; set; }
    }
}