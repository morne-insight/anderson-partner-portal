using AndersonAPI.Application.Common.Interfaces;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandModels", Version = "1.0")]

namespace AndersonAPI.Application.Countries.UpdateCountry
{
    public class UpdateCountryCommand : IRequest, ICommand
    {
        public UpdateCountryCommand(Guid id, Guid? regionId, string name, string description)
        {
            Id = id;
            RegionId = regionId;
            Name = name;
            Description = description;
        }

        public Guid Id { get; set; }
        public Guid? RegionId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}