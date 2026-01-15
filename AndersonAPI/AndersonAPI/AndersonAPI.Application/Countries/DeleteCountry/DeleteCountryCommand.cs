using AndersonAPI.Application.Common.Interfaces;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandModels", Version = "1.0")]

namespace AndersonAPI.Application.Countries.DeleteCountry
{
    public class DeleteCountryCommand : IRequest, ICommand
    {
        public DeleteCountryCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}