using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace AndersonAPI.Application.Dashboard
{
    public record DashboardOpportunityDto
    {
        public DashboardOpportunityDto()
        {
            OpportunityType = null!;
            Status = null!;
        }

        public string OpportunityType { get; set; }
        public string Status { get; set; }
        public DateOnly? Deadline { get; set; }

        public static DashboardOpportunityDto Create(string opportunityType, string status, DateOnly? deadline)
        {
            return new DashboardOpportunityDto
            {
                OpportunityType = opportunityType,
                Status = status,
                Deadline = deadline
            };
        }
    }
}