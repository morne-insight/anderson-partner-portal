using AndersonAPI.Application.Common.Interfaces;
using AndersonAPI.Domain;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandModels", Version = "1.0")]

namespace AndersonAPI.Application.Countries.SetStateCountry
{
    public class SetStateCountryCommand : IRequest, ICommand
    {
        public SetStateCountryCommand(Guid id, EntityState state)
        {
            Id = id;
            State = state;
        }

        public Guid Id { get; set; }
        public EntityState State { get; set; }
    }
}