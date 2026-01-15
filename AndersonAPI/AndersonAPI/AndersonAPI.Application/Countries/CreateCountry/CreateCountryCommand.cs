using AndersonAPI.Application.Common.Interfaces;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandModels", Version = "1.0")]

namespace AndersonAPI.Application.Countries.CreateCountry
{
    public class CreateCountryCommand : IRequest<Guid>, ICommand
    {
        public CreateCountryCommand(Guid? regionId, string name, string description)
        {
            RegionId = regionId;
            Name = name;
            Description = description;
        }

        public Guid? RegionId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}