using AndersonAPI.Application.Common.Interfaces;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandModels", Version = "1.0")]

namespace AndersonAPI.Application.Companies.AddLocationCompany
{
    public class AddLocationCompanyCommand : IRequest, ICommand
    {
        public AddLocationCompanyCommand(Guid id, string name, Guid regionId, Guid countryId, bool isHeadOffice)
        {
            Id = id;
            Name = name;
            RegionId = regionId;
            CountryId = countryId;
            IsHeadOffice = isHeadOffice;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid RegionId { get; set; }
        public Guid CountryId { get; set; }
        public bool IsHeadOffice { get; set; }
    }
}