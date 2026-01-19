using AndersonAPI.Domain;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace AndersonAPI.Application.Companies
{
    public record CompanyProfileDto
    {
        public CompanyProfileDto()
        {
            Name = null!;
            ShortDescription = null!;
            FullDescription = null!;
            WebsiteUrl = null!;
            ApplicationIdentityUsers = null!;
            Capabilities = null!;
            ServiceTypes = null!;
            Industries = null!;
            Contacts = null!;
            Locations = null!;
            Invites = null!;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }
        public string WebsiteUrl { get; set; }
        public int EmployeeCount { get; set; }
        public List<CompanyApplicationIdentityUserDto> ApplicationIdentityUsers { get; set; }
        public List<CompanyCapabilityDto> Capabilities { get; set; }
        public List<CompanyServiceTypeDto> ServiceTypes { get; set; }
        public List<CompanyIndustryDto> Industries { get; set; }
        public List<CompanyContactDto> Contacts { get; set; }
        public List<CompanyLocationDto> Locations { get; set; }
        public List<CompanyInviteDto> Invites { get; set; }
        public EntityState State { get; set; }

        public static CompanyProfileDto Create(
            Guid id,
            string name,
            string shortDescription,
            string fullDescription,
            string websiteUrl,
            int employeeCount,
            List<CompanyApplicationIdentityUserDto> applicationIdentityUsers,
            List<CompanyCapabilityDto> capabilities,
            List<CompanyServiceTypeDto> serviceTypes,
            List<CompanyIndustryDto> industries,
            List<CompanyContactDto> contacts,
            List<CompanyLocationDto> locations,
            List<CompanyInviteDto> invites,
            EntityState state)
        {
            return new CompanyProfileDto
            {
                Id = id,
                Name = name,
                ShortDescription = shortDescription,
                FullDescription = fullDescription,
                WebsiteUrl = websiteUrl,
                EmployeeCount = employeeCount,
                ApplicationIdentityUsers = applicationIdentityUsers,
                Capabilities = capabilities,
                ServiceTypes = serviceTypes,
                Industries = industries,
                Contacts = contacts,
                Locations = locations,
                Invites = invites,
                State = state
            };
        }
    }
}