using AndersonAPI.Application.Common.Interfaces;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandModels", Version = "1.0")]

namespace AndersonAPI.Application.Companies.CreateCompany
{
    public class CreateCompanyCommand : IRequest<Guid>, ICommand
    {
        public CreateCompanyCommand(string name,
            string shortDescription,
            string fullDescription,
            string websiteUrl,
            int employeeCount,
            List<Guid> capabilities,
            List<Guid> industries,
            Guid? serviceTypeId)
        {
            Name = name;
            ShortDescription = shortDescription;
            FullDescription = fullDescription;
            WebsiteUrl = websiteUrl;
            EmployeeCount = employeeCount;
            Capabilities = capabilities;
            Industries = industries;
            ServiceTypeId = serviceTypeId;
        }

        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }
        public string WebsiteUrl { get; set; }
        public int EmployeeCount { get; set; }
        public List<Guid> Capabilities { get; set; }
        public List<Guid> Industries { get; set; }
        public Guid? ServiceTypeId { get; set; }
    }
}