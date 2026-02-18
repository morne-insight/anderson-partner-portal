using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace AndersonAPI.Application.Dashboard
{
    public record DashboardPartnerDto
    {
        public DashboardPartnerDto()
        {
            Locations = null!;
            ServiceTypes = null!;
        }
        public List<DashboardLocationDto> Locations { get; set; }
        public List<DashboardServiceTypeDto> ServiceTypes { get; set; }

        public static DashboardPartnerDto Create(
            List<DashboardLocationDto> locations,
            List<DashboardServiceTypeDto> serviceTypes)
        {
            return new DashboardPartnerDto
            {
                Locations = locations
,
                ServiceTypes = serviceTypes
            };
        }
    }
}