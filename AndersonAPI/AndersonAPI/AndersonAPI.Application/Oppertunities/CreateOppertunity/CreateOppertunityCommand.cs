using AndersonAPI.Application.Common.Interfaces;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandModels", Version = "1.0")]

namespace AndersonAPI.Application.Oppertunities.CreateOppertunity
{
    public class CreateOppertunityCommand : IRequest<Guid>, ICommand
    {
        public CreateOppertunityCommand(string title,
            string shortDescription,
            string fullDescription,
            DateOnly? deadline,
            Guid oppertunityTypeId,
            Guid countryId,
            List<Guid> serviceTypes,
            List<Guid> capabilities,
            Guid companyId,
            List<Guid> industries)
        {
            Title = title;
            ShortDescription = shortDescription;
            FullDescription = fullDescription;
            Deadline = deadline;
            OppertunityTypeId = oppertunityTypeId;
            CountryId = countryId;
            ServiceTypes = serviceTypes;
            Capabilities = capabilities;
            CompanyId = companyId;
            Industries = industries;
        }

        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }
        public DateOnly? Deadline { get; set; }
        public Guid OppertunityTypeId { get; set; }
        public Guid CountryId { get; set; }
        public List<Guid> ServiceTypes { get; set; }
        public List<Guid> Capabilities { get; set; }
        public Guid CompanyId { get; set; }
        public List<Guid> Industries { get; set; }
    }
}