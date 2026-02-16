using AndersonAPI.Application.Common.Interfaces;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandModels", Version = "1.0")]

namespace AndersonAPI.Application.Companies.UpdateCompany
{
    public class UpdateCompanyCommand : IRequest, ICommand
    {
        public UpdateCompanyCommand(Guid id,
            string name,
            string shortDescription,
            string fullDescription,
            string websiteUrl,
            int employeeCount)
        {
            Id = id;
            Name = name;
            ShortDescription = shortDescription;
            FullDescription = fullDescription;
            WebsiteUrl = websiteUrl;
            EmployeeCount = employeeCount;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }
        public string WebsiteUrl { get; set; }
        public int EmployeeCount { get; set; }
    }
}