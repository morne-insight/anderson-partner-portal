using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace AndersonAPI.Application.Dashboard
{
    public record DashboardPartnerDto
    {
        public DashboardPartnerDto()
        {
            ServiceType = null!;
            Locations = null!;
        }

        public string ServiceType { get; set; }
        public List<DashboardLocationDto> Locations { get; set; }

        public static DashboardPartnerDto Create(string serviceType, List<DashboardLocationDto> locations)
        {
            return new DashboardPartnerDto
            {
                ServiceType = serviceType,
                Locations = locations
            };
        }
    }
}