using AndersonAPI.Application.Common.Interfaces;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandModels", Version = "1.0")]

namespace AndersonAPI.Application.CompanyProfiles.CreateCompanyProfile
{
    public class CreateCompanyProfileCommand : IRequest<Guid>, ICommand
    {
        public CreateCompanyProfileCommand(string name,
            string shortDescription,
            string description,
            string websiteUrl,
            int employeeCount)
        {
            Name = name;
            ShortDescription = shortDescription;
            Description = description;
            WebsiteUrl = websiteUrl;
            EmployeeCount = employeeCount;
        }

        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string WebsiteUrl { get; set; }
        public int EmployeeCount { get; set; }
    }
}