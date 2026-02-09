using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace AndersonAPI.Application.Dashboard
{
    public record DashboardLocationDto
    {
        public DashboardLocationDto()
        {
            Country = null!;
            Region = null!;
        }

        public string Country { get; set; }
        public string Region { get; set; }

        public static DashboardLocationDto Create(string country, string region)
        {
            return new DashboardLocationDto
            {
                Country = country,
                Region = region
            };
        }
    }
}