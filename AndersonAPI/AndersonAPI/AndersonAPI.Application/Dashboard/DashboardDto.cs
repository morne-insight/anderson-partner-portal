using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace AndersonAPI.Application.Dashboard
{
    public record DashboardDto
    {
        public DashboardDto()
        {
            Opportunities = null!;
            Partners = null!;
        }

        public List<DashboardOpportunityDto> Opportunities { get; set; }
        public List<DashboardPartnerDto> Partners { get; set; }

        public static DashboardDto Create(List<DashboardOpportunityDto> opportunities, List<DashboardPartnerDto> partners)
        {
            return new DashboardDto
            {
                Opportunities = opportunities,
                Partners = partners
            };
        }
    }
}