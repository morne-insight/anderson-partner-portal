using AndersonAPI.Application.Common.Interfaces;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandModels", Version = "1.0")]

namespace AndersonAPI.Application.Companies.SetHeadOfficeCompany
{
    public class SetHeadOfficeCompanyCommand : IRequest, ICommand
    {
        public SetHeadOfficeCompanyCommand(Guid id, Guid locationId)
        {
            Id = id;
            LocationId = locationId;
        }

        public Guid Id { get; set; }
        public Guid LocationId { get; set; }
    }
}