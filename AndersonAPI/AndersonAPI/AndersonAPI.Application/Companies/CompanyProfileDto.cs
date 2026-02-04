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
            ServiceTypeName = null!;
            ServiceSubTypes = null!;
            Capabilities = null!;
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
        public Guid? ServiceTypeId { get; set; }
        public string ServiceTypeName { get; set; }
        public List<CompanyServiceSubTypeDto> ServiceSubTypes { get; set; }
        public List<CompanyIdentityUserDto> ApplicationIdentityUsers { get; set; }
        public List<CompanyCapabilityDto> Capabilities { get; set; }
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
            List<CompanyIdentityUserDto> applicationIdentityUsers,
            Guid? serviceTypeId,
            string serviceTypeName,
            List<CompanyServiceSubTypeDto> serviceSubTypes,
            List<CompanyCapabilityDto> capabilities,
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
                ServiceTypeId = serviceTypeId,
                ServiceTypeName = serviceTypeName,
                ServiceSubTypes = serviceSubTypes,
                Capabilities = capabilities,
                Industries = industries,
                Contacts = contacts,
                Locations = locations,
                Invites = invites,
                State = state
            };
        }
    }
}