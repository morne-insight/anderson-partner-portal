using AndersonAPI.Application.Common.Interfaces;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandModels", Version = "1.0")]

namespace AndersonAPI.Application.Opportunities.UpdateFullOpportunity
{
    public class UpdateFullOpportunityCommand : IRequest, ICommand
    {
        public UpdateFullOpportunityCommand(Guid id,
            string title,
            string shortDescription,
            string fullDescription,
            DateOnly? deadline,
            Guid opportunityTypeId,
            Guid countryId,
            List<Guid> serviceTypes,
            List<Guid> capabilities,
            List<Guid> industries)
        {
            Id = id;
            Title = title;
            ShortDescription = shortDescription;
            FullDescription = fullDescription;
            Deadline = deadline;
            OpportunityTypeId = opportunityTypeId;
            CountryId = countryId;
            ServiceTypes = serviceTypes;
            Capabilities = capabilities;
            Industries = industries;
        }

        public Guid Id { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }
        public DateOnly? Deadline { get; set; }
        public Guid OpportunityTypeId { get; set; }
        public Guid CountryId { get; set; }
        public List<Guid> ServiceTypes { get; set; }
        public List<Guid> Capabilities { get; set; }
        public List<Guid> Industries { get; set; }
    }
}