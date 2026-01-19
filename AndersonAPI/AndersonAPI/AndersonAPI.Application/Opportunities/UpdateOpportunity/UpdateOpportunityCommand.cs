using AndersonAPI.Application.Common.Interfaces;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandModels", Version = "1.0")]

namespace AndersonAPI.Application.Opportunities.UpdateOpportunity
{
    public class UpdateOpportunityCommand : IRequest, ICommand
    {
        public UpdateOpportunityCommand(Guid id,
            string title,
            string shortDescription,
            string fullDescription,
            DateOnly? deadline,
            Guid opportunityTypeId,
            Guid countryId)
        {
            Id = id;
            Title = title;
            ShortDescription = shortDescription;
            FullDescription = fullDescription;
            Deadline = deadline;
            OpportunityTypeId = opportunityTypeId;
            CountryId = countryId;
        }

        public Guid Id { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }
        public DateOnly? Deadline { get; set; }
        public Guid OpportunityTypeId { get; set; }
        public Guid CountryId { get; set; }
    }
}